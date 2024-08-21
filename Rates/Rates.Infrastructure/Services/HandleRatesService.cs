using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rates.Infrastructure.Exceptions;
using Serilog;

namespace Rates.Infrastructure.Services;

public class HandleRatesService : IHandleRatesService
{
    private readonly IDbRepository _repository;
    private readonly ILogger<HandleRatesService> _logger;

    public HandleRatesService(IDbRepository repository, ILogger<HandleRatesService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task UploadExchangeRates(DateTime date)
    {
        _logger.LogInformation("Fetching exchange rates for date: {Date}", date);

        var rates = await GetCurrencyRatesFromApi($"https://api.nbrb.by/exrates/rates?ondate={date.Year}-{date.Month}-{date.Day}&periodicity=0");
        
        foreach (var rate in rates)
        {
            rate.Date = DateTime.SpecifyKind(rate.Date, DateTimeKind.Utc);
        }

        _logger.LogInformation("Checking if rates already exist in the database.");

        bool exists = await _repository.Get<CurrencyRate>(r => r.Cur_ID == rates.First().Cur_ID && r.Date == rates.First().Date).AnyAsync();

        if (!exists)
        {
            _logger.LogInformation("Rates do not exist. Adding new rates.");

            foreach (var rate in rates)
            {
                await _repository.Add(rate);
            }
            await _repository.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("Rate for this day is already in database.");
            throw new IncorrectDataException("Rate for this day is already in database");
        }
    }

    private static async Task<List<CurrencyRate>?> GetCurrencyRatesFromApi(string apiUrl)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Log.Error("Invalid date to get rates from API.");
            throw new IncorrectDataException("Invalid date to get rates");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CurrencyRate>>(jsonResponse);
    }

    public async Task<IQueryable<CurrencyRate>> GetExchangeRates(int day, int month, int year, int curId)
    {
        var date = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        
        var rates = _repository.Get<CurrencyRate>()
            .Where(r => r.Cur_ID == curId && r.Date == date);
        
        if (!rates.Any())
        {
            _logger.LogWarning("No exchange rates found for date: {Date} and currency ID: {CurId}", date, curId);
            throw new IncorrectDataException("No exchange rates found for the specified date and currency ID.");
        }
    
        return rates; 
    }
}

using System.Text.Json;
using exchange_rates.Exceptions;
using exchange_rates.Models;
using exchange_rates.Repository.Interfaces;
using exchange_rates.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace exchange_rates.Services;

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
        var rates = await GetCurrencyRatesFromApi($"https://api.nbrb.by/exrates/rates?ondate={date.Year}-{date.Month}-{date.Day}&periodicity=0");
        
        foreach (var rate in rates)
        {
            rate.Date = DateTime.SpecifyKind(rate.Date, DateTimeKind.Utc);
        }

        bool exists = await _repository.Get<CurrencyRate>(r => r.Cur_ID == rates[0].Cur_ID && r.Date == rates[0].Date).AnyAsync();
    
        if (!exists)
        {
            foreach (var rate in rates)
            {
                await _repository.Add(rate);
            }
            await _repository.SaveChangesAsync();
        }
        else
        {
            throw new IncorrectDataException("Rate for this day is already in database");
        }
    }

    private static async Task<List<CurrencyRate>?> GetCurrencyRatesFromApi(string apiUrl)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
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
            throw new IncorrectDataException("No exchange rates found for the specified date and currency ID.");
        }
    
        return rates; 
    }

}
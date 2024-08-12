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

    public async Task UploadExchangeRates(int day, int month, int year)
    {
        if (year > 2024)
            throw new IncorrectDataException("Year is invalid");
        if (day is < 1 or > 31)
            throw new IncorrectDataException("Day is invalid");
        if (month is < 1 or > 12)
            throw new IncorrectDataException("Month is invalid");
    
        var rates = await GetCurrencyRatesFromApi($"https://api.nbrb.by/exrates/rates?ondate={year}-{month}-{day}&periodicity=0");

        // Убедитесь, что даты в формате UTC перед любой операцией с ними
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
}
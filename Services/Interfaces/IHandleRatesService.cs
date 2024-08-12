using exchange_rates.Models;

namespace exchange_rates.Services.Interfaces;

public interface IHandleRatesService
{
    Task UploadExchangeRates(int day, int month, int year);
    Task<IQueryable<CurrencyRate>> GetExchangeRates(int day, int month, int year, int curId);
}
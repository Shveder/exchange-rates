using Rates.Core.Models;

namespace Rates.Infrastructure.Services.Interfaces;

public interface IHandleRatesService
{
    Task UploadExchangeRates(DateTime date);
    Task<IQueryable<CurrencyRate>> GetExchangeRates(int day, int month, int year, int curId);
}
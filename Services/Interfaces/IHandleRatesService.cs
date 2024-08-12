namespace exchange_rates.Services.Interfaces;

public interface IHandleRatesService
{
    Task UploadExchangeRates(int day, int month, int year);
}
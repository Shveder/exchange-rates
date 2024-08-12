using exchange_rates.Models.Interface;

namespace exchange_rates.Models;

public class CurrencyRate : IModels
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public DateTime Date { get; set; }
    public decimal Rate { get; set; }
    public bool Status { get; set; }

}
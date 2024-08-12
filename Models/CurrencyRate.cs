using System.ComponentModel.DataAnnotations;
using exchange_rates.Models.Interface;

namespace exchange_rates.Models;

public class CurrencyRate : IModels
{
    [Key]public Guid Id { get; set; }
    [Required]  public int Cur_ID { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public string Cur_Abbreviation { get; set; }
    [Required] public int Cur_Scale { get; set; }
    [Required] public string Cur_Name { get; set; }
    [Required] public decimal Cur_OfficialRate { get; set; }

}
using System.ComponentModel.DataAnnotations;
using Rates.Core.Models.Interface;

namespace Rates.Core.Models;

public class CurrencyRate : IModels
{
    /// <summary>
    /// Id of rate.
    /// </summary>
    [Key]public Guid Id { get; set; }
    /// <summary>
    /// Id of currency.
    /// </summary>
    [Required]  public int Cur_ID { get; set; }
    /// <summary>
    /// Date of rate.
    /// </summary>
    [Required] public DateTime Date { get; set; }
    /// <summary>
    /// Abbreviation of currency.
    /// </summary>
    [Required] public string Cur_Abbreviation { get; set; }
    /// <summary>
    /// Scale of currency.
    /// </summary>
    [Required] public int Cur_Scale { get; set; }
    /// <summary>
    /// Name of currency.
    /// </summary>
    [Required] public string Cur_Name { get; set; }
    /// <summary>
    /// Official rate of currency.
    /// </summary>
    [Required] public decimal Cur_OfficialRate { get; set; }

}
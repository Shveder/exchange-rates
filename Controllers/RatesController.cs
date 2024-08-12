using exchange_rates.Data;
using exchange_rates.Models;
using Microsoft.AspNetCore.Mvc;

namespace exchange_rates.Controllers;

[ApiController]
[Route("RatesController")]
public class RatesController : ControllerBase
{

    [HttpGet]
    [Route("GetExchangeRates")]
    public async Task<IActionResult> GetExchangeRates()
    {

        return Ok("exchagerate");
    }
}
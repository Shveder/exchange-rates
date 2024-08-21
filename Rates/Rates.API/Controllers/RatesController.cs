using Rates.Infrastructure.Services.Interfaces;

namespace Rates.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesController : ControllerBase
{

    private readonly ILogger<RatesController> _logger;
    private readonly IHandleRatesService _ratesService;

    public RatesController(ILogger<RatesController> logger, IHandleRatesService ratesService)
    {
        _logger = logger;
        _ratesService = ratesService;
    }
    
    /// <summary>
    ///     Upload exchange rates
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /Rates/UploadExchangeRates
    /// </remarks>
    /// <returns>
    ///     200 OK
    /// </returns>
    /// <response code="200">Rates is uploaded.</response>
    /// <response code="422">Invalid input data.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [Route("UploadExchangeRates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadExchangeRates([FromBody] DateTime date)
    {

        _logger.LogInformation("Received request to upload exchange rates for date: {Date}", date);
        await _ratesService.UploadExchangeRates(date);
        const string commonAnswer = "Successfully uploaded exchange rates for date: {Date}";
        _logger.LogInformation(commonAnswer, date);
        return Ok(commonAnswer);
    }
    
    /// <summary>
    ///     Get exchange rate
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /Rates/GetExchangeRates
    /// </remarks>
    /// <returns>
    ///     200 OK
    /// </returns>
    /// <response code="200">Rates is got.</response>
    /// <response code="422">Invalid input data.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [Route("GetExchangeRates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetExchangeRates(int day, int month, int year, int Cur_ID)
    {
        _logger.LogInformation("Exchange rate is got");    
        return Ok(await _ratesService.GetExchangeRates(day, month, year,Cur_ID));
    }
}
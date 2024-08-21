using System.Linq.Expressions;
using System.Net.Http.Json;
using MockQueryable.EntityFrameworkCore;

namespace Rates.Tests;

public class HandleRatesServiceTests
{
    private readonly Mock<IDbRepository> _repositoryMock;
    private readonly Mock<ILogger<HandleRatesService>> _loggerMock;
    private readonly HandleRatesService _service;
    private readonly HttpClient _httpClient;

    public HandleRatesServiceTests()
    {
        _repositoryMock = new Mock<IDbRepository>();
        _loggerMock = new Mock<ILogger<HandleRatesService>>();
        
        var handlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(handlerMock.Object);

        _service = new HandleRatesService(_repositoryMock.Object, _loggerMock.Object);
    }
    
    [Fact]
    public async Task UploadExchangeRates_ShouldThrowException_WhenRatesAlreadyExist()
    {
        // Arrange
        var date = new DateTime(2023, 8, 20);
        var existingRates = new List<CurrencyRate> { new CurrencyRate { Cur_ID = 1, Date = date } };

        _repositoryMock.Setup(repo => repo.Get<CurrencyRate>(It.IsAny<Expression<Func<CurrencyRate, bool>>>()))
            .Returns(existingRates.AsQueryable().BuildMock());

        // Act & Assert
        await Assert.ThrowsAsync<IncorrectDataException>(() => _service.UploadExchangeRates(date));
    }

    [Fact]
    public async Task GetExchangeRates_ShouldReturnRates_WhenRatesExist()
    {
        // Arrange
        var date = new DateTime(2023, 8, 20, 0, 0, 0, DateTimeKind.Utc);
        var curId = 1;
        var rates = new List<CurrencyRate> { new CurrencyRate { Cur_ID = curId, Date = date } }.AsQueryable();

        _repositoryMock.Setup(repo => repo.Get<CurrencyRate>())
            .Returns(rates.BuildMock());

        // Act
        var result = await _service.GetExchangeRates(date.Day, date.Month, date.Year, curId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rates.Count(), result.Count());
    }

    [Fact]
    public async Task GetExchangeRates_ShouldThrowException_WhenRatesDoNotExist()
    {
        // Arrange
        var date = new DateTime(2023, 8, 20, 0, 0, 0, DateTimeKind.Utc);
        var curId = 1;

        _repositoryMock.Setup(repo => repo.Get<CurrencyRate>())
            .Returns(Enumerable.Empty<CurrencyRate>().AsQueryable().BuildMock());

        // Act & Assert
        await Assert.ThrowsAsync<IncorrectDataException>(() => _service.GetExchangeRates(date.Day, date.Month, date.Year, curId));
    }
}

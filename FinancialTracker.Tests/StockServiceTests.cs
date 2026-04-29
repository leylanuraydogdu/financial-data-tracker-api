using FinancialTracker.ExternalApis;
using FinancialTracker.Models;
using FinancialTracker.Repositories;
using FinancialTracker.Services;
using Moq;
using Xunit;

namespace FinancialTracker.Tests;

public class StockServiceTests
{
    private readonly Mock<IStockRepository> _mockRepository;
    private readonly Mock<IAlphaVantageService> _mockAlphaVantageService;
    private readonly StockService _stockService;

    public StockServiceTests()
    {
        _mockRepository = new Mock<IStockRepository>();
        _mockAlphaVantageService = new Mock<IAlphaVantageService>();
        _stockService = new StockService(_mockRepository.Object, _mockAlphaVantageService.Object);
    }

    [Fact]
    public async Task GetAllStocksAsync_ShouldReturnAllStocks()
    {
        // Arrange
        var mockStocks = new List<Stock>
        {
            new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." },
            new Stock { Id = 2, Symbol = "MSFT", CompanyName = "Microsoft" }
        };
        _mockRepository.Setup(repo => repo.GetAllStocksAsync()).ReturnsAsync(mockStocks);

        // Act
        var result = await _stockService.GetAllStocksAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, s => s.Symbol == "AAPL");
    }

    [Fact]
    public async Task AddStockAsync_WhenStockExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var existingSymbol = "AAPL";
        _mockRepository.Setup(repo => repo.GetStockBySymbolAsync(existingSymbol))
            .ReturnsAsync(new Stock { Id = 1, Symbol = existingSymbol });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _stockService.AddStockAsync(existingSymbol, "Apple Inc."));
        
        Assert.Equal($"Stock with symbol '{existingSymbol}' is already in the watchlist.", exception.Message);
    }
}

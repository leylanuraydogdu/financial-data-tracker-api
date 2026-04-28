using FinancialTracker.Models;

namespace FinancialTracker.ExternalApis;

/// <summary>
/// Service interface to fetch data from Alpha Vantage API.
/// </summary>
public interface IAlphaVantageService
{
    Task<StockPrice?> GetLatestPriceAsync(string symbol);
}

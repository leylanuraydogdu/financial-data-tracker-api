using FinancialTracker.Models;

namespace FinancialTracker.Repositories;

/// <summary>
/// Interface for stock data operations.
/// </summary>
public interface IStockRepository
{
    Task<IEnumerable<Stock>> GetAllStocksAsync();
    Task<Stock?> GetStockByIdAsync(int id);
    Task<Stock?> GetStockBySymbolAsync(string symbol);
    Task<Stock> AddStockAsync(Stock stock);
    Task DeleteStockAsync(Stock stock);
    Task AddStockPriceAsync(StockPrice price);
    Task<IEnumerable<Stock>> GetTopPerformersAsync(int count);
}

using FinancialTracker.Models;

namespace FinancialTracker.Services;

/// <summary>
/// Service interface for stock operations and business logic.
/// </summary>
public interface IStockService
{
    Task<IEnumerable<Stock>> GetAllStocksAsync();
    Task<Stock?> GetStockByIdAsync(int id);
    Task<Stock> AddStockAsync(string symbol, string companyName);
    Task<bool> RemoveStockAsync(int id);
}

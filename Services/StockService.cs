using FinancialTracker.Models;
using FinancialTracker.Repositories;

namespace FinancialTracker.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;

    public StockService(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<IEnumerable<Stock>> GetAllStocksAsync()
    {
        return await _stockRepository.GetAllStocksAsync();
    }

    public async Task<Stock?> GetStockByIdAsync(int id)
    {
        return await _stockRepository.GetStockByIdAsync(id);
    }

    public async Task<Stock> AddStockAsync(string symbol, string companyName)
    {
        // 1. Hisse senedi zaten ekli mi kontrol et
        var existingStock = await _stockRepository.GetStockBySymbolAsync(symbol);
        if (existingStock != null)
        {
            throw new InvalidOperationException($"Stock with symbol '{symbol}' is already in the watchlist.");
        }

        // 2. Yeni hisseyi oluştur
        var stock = new Stock
        {
            Symbol = symbol.ToUpper(),
            CompanyName = companyName,
            AddedAt = DateTime.UtcNow
        };

        // 3. Veritabanına kaydet
        var savedStock = await _stockRepository.AddStockAsync(stock);
        
        // TODO: İleriki adımda burada Alpha Vantage API'den geçmiş verileri çekeceğiz.
        
        return savedStock;
    }

    public async Task<bool> RemoveStockAsync(int id)
    {
        var stock = await _stockRepository.GetStockByIdAsync(id);
        if (stock == null)
            return false;

        await _stockRepository.DeleteStockAsync(stock);
        return true;
    }
}

using Microsoft.EntityFrameworkCore;
using FinancialTracker.Data;
using FinancialTracker.Models;

namespace FinancialTracker.Repositories;

// Design Pattern: Repository Pattern
// Neden kullanıldı: Veri erişim mantığını (Entity Framework Core sorgularını) 
// iş mantığından (Controller/Service) ayırmak için kullanıldı. 
// Bu sayede veritabanı değişiklikleri sadece bu sınıfta yapılır.
public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stock>> GetAllStocksAsync()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task<Stock?> GetStockByIdAsync(int id)
    {
        return await _context.Stocks
            .Include(s => s.PriceHistory.OrderByDescending(p => p.Date))
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock> AddStockAsync(Stock stock)
    {
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task DeleteStockAsync(Stock stock)
    {
        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
    }

    public async Task AddStockPriceAsync(StockPrice price)
    {
        _context.StockPrices.Add(price);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Stock>> GetTopPerformersAsync(int count)
    {
        // En güncel fiyata göre son eklenen hisseleri getirir
        // Analitik endpoint için basit bir getirme işlemi
        return await _context.Stocks
            .Include(s => s.PriceHistory.OrderByDescending(p => p.Date).Take(1))
            .Take(count)
            .ToListAsync();
    }
}

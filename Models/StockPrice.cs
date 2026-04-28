namespace FinancialTracker.Models;

/// <summary>
/// Represents the daily price data for a specific stock.
/// </summary>
public class StockPrice
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public DateTime Date { get; set; }
    public decimal OpenPrice { get; set; }
    public decimal ClosePrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public long Volume { get; set; }

    // Navigation property
    public Stock Stock { get; set; } = null!;
}

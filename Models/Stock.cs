namespace FinancialTracker.Models;

/// <summary>
/// Represents a stock that the user is tracking in their watchlist.
/// </summary>
public class Stock
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public List<StockPrice> PriceHistory { get; set; } = new();
}

namespace FinancialTracker.DTOs;

public class StockAnalyticsDto
{
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal LatestPrice { get; set; }
    public long Volume { get; set; }
}

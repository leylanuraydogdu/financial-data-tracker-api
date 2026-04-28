using FinancialTracker.DTOs;
using FinancialTracker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IStockRepository _stockRepository;

    public AnalyticsController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    // GET: api/analytics/top-performers
    // İstenilen analitik kuralı: Fiyatı en yüksek olan (veya en güncel) hisseleri getir
    [HttpGet("top-performers")]
    public async Task<IActionResult> GetTopPerformers([FromQuery] int count = 5)
    {
        var topStocks = await _stockRepository.GetTopPerformersAsync(count);
        
        var result = topStocks
            .Where(s => s.PriceHistory.Any()) // Sadece fiyat bilgisi çekilebilmiş olanları al
            .Select(s => new StockAnalyticsDto
            {
                Symbol = s.Symbol,
                CompanyName = s.CompanyName,
                LatestPrice = s.PriceHistory.First().ClosePrice,
                Volume = s.PriceHistory.First().Volume
            })
            .OrderByDescending(dto => dto.LatestPrice) // En pahalıdan ucuza doğru sırala
            .ToList();

        return Ok(result);
    }
}

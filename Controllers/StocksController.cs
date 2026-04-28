using FinancialTracker.DTOs;
using FinancialTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IStockService _stockService;

    public StocksController(IStockService stockService)
    {
        _stockService = stockService;
    }

    // GET: api/stocks
    [HttpGet]
    public async Task<IActionResult> GetAllStocks()
    {
        var stocks = await _stockService.GetAllStocksAsync();
        
        // Map Entities to DTOs
        var dtos = stocks.Select(s => new StockDto
        {
            Id = s.Id,
            Symbol = s.Symbol,
            CompanyName = s.CompanyName,
            AddedAt = s.AddedAt,
            LatestPrice = s.PriceHistory.OrderByDescending(p => p.Date).FirstOrDefault()?.ClosePrice
        });

        return Ok(dtos);
    }

    // GET: api/stocks/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockById(int id)
    {
        var stock = await _stockService.GetStockByIdAsync(id);
        
        if (stock == null)
            return NotFound(new { message = $"Stock with ID {id} not found." });

        var dto = new StockDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            AddedAt = stock.AddedAt,
            LatestPrice = stock.PriceHistory.OrderByDescending(p => p.Date).FirstOrDefault()?.ClosePrice
        };

        return Ok(dto);
    }

    // POST: api/stocks
    [HttpPost]
    public async Task<IActionResult> AddStock([FromBody] CreateStockRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var stock = await _stockService.AddStockAsync(request.Symbol, request.CompanyName);
            
            var dto = new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                AddedAt = stock.AddedAt
            };

            return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, dto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding the stock.", details = ex.Message });
        }
    }

    // DELETE: api/stocks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveStock(int id)
    {
        var result = await _stockService.RemoveStockAsync(id);
        
        if (!result)
            return NotFound(new { message = $"Stock with ID {id} not found." });

        return NoContent(); // 204 Success but no content
    }
}

using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.DTOs;

public class CreateStockRequest
{
    [Required(ErrorMessage = "Symbol is required.")]
    [StringLength(10, ErrorMessage = "Symbol cannot exceed 10 characters.")]
    public string Symbol { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
    public string CompanyName { get; set; } = string.Empty;
}

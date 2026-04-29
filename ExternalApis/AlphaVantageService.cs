using System.Globalization;
using System.Text.Json;
using FinancialTracker.Models;

namespace FinancialTracker.ExternalApis;

public class AlphaVantageService : IAlphaVantageService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AlphaVantageService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<StockPrice?> GetLatestPriceAsync(string symbol)
    {
        var apiKey = _configuration["AlphaVantage:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("Alpha Vantage API Key is missing. Please add it to appsettings.json.");
        }

        var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={apiKey}";
        
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        
        using var doc = JsonDocument.Parse(jsonString);
        var root = doc.RootElement;

        // Eger API limiti dolduysa veya sembol gecersizse "Global Quote" nesnesi donmez
        if (!root.TryGetProperty("Global Quote", out var quoteElement) || !quoteElement.EnumerateObject().Any())
        {
            return null;
        }

        try 
        {
            return new StockPrice
            {
                Date = DateTime.Parse(quoteElement.GetProperty("07. latest trading day").GetString()!, CultureInfo.InvariantCulture),
                OpenPrice = decimal.Parse(quoteElement.GetProperty("02. open").GetString()!, CultureInfo.InvariantCulture),
                HighPrice = decimal.Parse(quoteElement.GetProperty("03. high").GetString()!, CultureInfo.InvariantCulture),
                LowPrice = decimal.Parse(quoteElement.GetProperty("04. low").GetString()!, CultureInfo.InvariantCulture),
                ClosePrice = decimal.Parse(quoteElement.GetProperty("05. price").GetString()!, CultureInfo.InvariantCulture),
                Volume = long.Parse(quoteElement.GetProperty("06. volume").GetString()!, CultureInfo.InvariantCulture)
            };
        }
        catch
        {
            // Parse hatasi olursa (beklenmeyen veri formati) null don
            return null;
        }
    }
}

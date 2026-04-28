using Microsoft.EntityFrameworkCore;
using FinancialTracker.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database - SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<FinancialTracker.Repositories.IStockRepository, FinancialTracker.Repositories.StockRepository>();

// Services
builder.Services.AddScoped<FinancialTracker.Services.IStockService, FinancialTracker.Services.StockService>();

// External APIs
builder.Services.AddHttpClient<FinancialTracker.ExternalApis.IAlphaVantageService, FinancialTracker.ExternalApis.AlphaVantageService>();

var app = builder.Build();

// Apply pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

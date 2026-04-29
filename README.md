# Financial Data Tracker

## Project Purpose
This is a Financial Data Tracker Web API developed for the Rasyonet Software Engineering Internship technical assessment. It serves as a simple backend system for a Stock Watchlist tool where users can track stock symbols, automatically fetch their latest daily prices from an external source, and view analytical insights.

## Technologies Used
- **Framework:** .NET 8 Web API
- **Database:** SQLite (chosen for its portability and ease of setup without requiring an external database server installation)
- **ORM:** Entity Framework Core
- **External API:** Alpha Vantage (used for fetching global quotes of stocks)
- **Containerization:** Docker
- **Testing:** xUnit & Moq

## Design Patterns Used
### Repository Pattern
The **Repository Pattern** is used to abstract the data access layer (Entity Framework Core) away from the business logic layer (Services). 
- **Why?** This ensures Separation of Concerns. The Controller and Service do not need to know about `DbContext` or SQL queries. It also makes the code easier to test and maintain. If we ever decide to switch from SQLite to PostgreSQL or MongoDB, we only need to change the Repository implementation.
- **Where?** See `Repositories/IStockRepository.cs` and `Repositories/StockRepository.cs`.

### Dependency Injection (DI)
Built-in .NET DI is used throughout the application to inject Repositories into Services, and Services into Controllers.

## Endpoints
1. `GET /api/stocks` - List all tracked stocks
2. `GET /api/stocks/{id}` - Get a specific stock and its price history
3. `POST /api/stocks` - Add a new stock to the watchlist (Automatically fetches current price from Alpha Vantage)
4. `DELETE /api/stocks/{id}` - Remove a stock from the watchlist
5. `GET /api/analytics/top-performers?count=5` - Analytical endpoint that returns the most valuable stocks based on their latest fetched closing price.

## Setup & Run Instructions

### Prerequisites
- .NET 8 SDK
- An Alpha Vantage API Key (Get a free one at [alphavantage.co](https://www.alphavantage.co/))

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/leylanuraydogdu/financial-data-tracker-api.git
   cd financial-data-tracker-api
   ```
2. Set your Alpha Vantage API key:
   Open `appsettings.json` and replace `"demo"` with your actual API key:
   ```json
   "AlphaVantage": {
     "ApiKey": "YOUR_API_KEY_HERE"
   }
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
5. Open your browser and navigate to the Swagger UI to test the endpoints:
   - `http://localhost:<port>/swagger/index.html` (Check the terminal output for the exact port)

### Running Unit Tests
A separate xUnit test project is included to verify the business logic. To run the tests:
```bash
cd FinancialTracker.Tests
dotnet test
```

### Running with Docker (Bonus)
You can run this application entirely within a Docker container without needing to install the .NET SDK.
1. Build the Docker image:
   ```bash
   docker build -t financial-tracker-api .
   ```
2. Run the container:
   ```bash
   docker run -d -p 8080:8080 --name financial-tracker financial-tracker-api
   ```
3. Access the API at `http://localhost:8080/swagger`.

## Notes
- **Database Creation:** The SQLite database (`financialtracker.db`) is automatically created upon application startup if it does not exist (`db.Database.EnsureCreated()`).
- **Error Handling:** A custom `ExceptionMiddleware` is implemented to ensure no raw stack traces are exposed to the client.

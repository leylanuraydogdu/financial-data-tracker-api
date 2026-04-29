# 📈 Financial Data Tracker API


Kullanıcıların istedikleri hisse senetlerini takip etmelerini, fiyatlarını otomatik çekmelerini ve bu hisseleri performanslarına göre analiz etmelerini sağlayan sade ve temiz bir sistem tasarlamaya çalıştım. 🚀

## 🛠️ Neler Kullandım?

- **Framework:** .NET 8 Web API 
- **Veritabanı:** SQLite 
- **ORM:** Entity Framework Core
- **Dış API (Veri Kaynağı):** Alpha Vantage 🌐 (Hisselerin güncel fiyatlarını çekmek için)
- **Konteyner Mimarisi:** Docker 🐳 (Bonus)
- **Test:** xUnit & Moq 🧪 (Bonus)

## 🏗️ Mimari ve Tasarım Kararları

Projeyi kodlarken iş mantığı ile veritabanı işlemlerini birbirine karıştırmamaya özen gösterdim. 

### Repository Pattern 🗂️
Veritabanı (Entity Framework Core) ile Controller/Service arasındaki bağı koparmak için **Repository Pattern** kullandım.
- **Neden?** Separation of Concerns (Sorumlulukların Ayrılığı) prensibi gereği, veritabanı sorgularının Controller veya Service içinde olmasını istemedim. Yarın bir gün SQLite yerine MongoDB veya PostgreSQL kullanmam gerekirse, sadece Repository katmanını değiştirmem yeterli olacak.
- **Nerede görebilirsiniz?** `Repositories/IStockRepository.cs` ve `Repositories/StockRepository.cs` dosyalarında.

Ayrıca `.NET`'in kendi **Dependency Injection (DI)** yapısını kullanarak servisleri ve repository'leri projeye entegre ettim. Olası çökmeleri kullanıcıya çirkin hatalar (raw exceptions) olarak yansıtmamak için de global bir **ExceptionMiddleware** yazdım.

## 📡 API Uç Noktaları (Endpoints)

1. `GET /api/stocks` 📋 Takip listesindeki tüm hisseleri getirir.
2. `GET /api/stocks/{id}` 🔍 Belirli bir hissenin detayını ve fiyat geçmişini getirir.
3. `POST /api/stocks` ➕ Takip listesine yeni bir hisse ekler (Eklerken Alpha Vantage API'sine bağlanıp anlık fiyatını da çeker ve veritabanına kaydeder).
4. `DELETE /api/stocks/{id}` 🗑️ Hisseyi listeden siler.
5. `GET /api/analytics/top-performers?count=5` 🏆 **(Analitik Endpoint)** Sistemdeki en değerli hisseleri son fiyatlarına göre sıralayıp getirir.

## 🚀 Projeyi Nasıl Çalıştırırsınız?

### Gereksinimler
- Bilgisayarınızda .NET 8 SDK kurulu olmalı.
- Alpha Vantage API Key almalısınız .

### Adım Adım Kurulum
1. Repoyu bilgisayarınıza indirin:
   ```bash
   git clone https://github.com/leylanuraydogdu/financial-data-tracker-api.git
   cd financial-data-tracker-api
   ```
2. API Key'inizi ekleyin:
   Proje içindeki `appsettings.json` dosyasını açın ve `"demo"` yazan yere kendi API anahtarınızı yapıştırın.
   ```json
   "AlphaVantage": {
     "ApiKey": "BURAYA_YAZIN"
   }
   ```
3. Projeyi derleyin ve çalıştırın:
   ```bash
   dotnet build
   dotnet run
   ```
4. Tarayıcınızda Swagger arayüzünü açın (Konsolda yazan porta göre):
   `http://localhost:<port>/swagger/index.html`

> 💡 **Not:** Veritabanı (`financialtracker.db`) projeyi ilk çalıştırdığınızda otomatik olarak oluşacaktır. Sizin ekstra bir migration yapmanıza gerek yok!

## 🧪 Birim Testleri (Unit Tests)

Projede iş mantığının (Business Logic) doğru çalıştığından emin olmak için xUnit ile testler yazdım. Testleri çalıştırmak için:
```bash
cd FinancialTracker.Tests
dotnet test
```

## 🐳 Docker ile Çalıştırma (Alternatif)

Sisteminizde .NET kurulu olmasa bile projeyi tek komutla Docker üzerinden ayağa kaldırabilirsiniz!
1. İmajı derleyin:
   ```bash
   docker build -t financial-tracker-api .
   ```
2. Konteyneri başlatın:
   ```bash
   docker run -d -p 8080:8080 --name financial-tracker financial-tracker-api
   ```
3. Tarayıcınızda `http://localhost:8080/swagger` adresine giderek sistemi kullanmaya başlayabilirsiniz.

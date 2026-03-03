# VAT Traders Billing Solution

This repository now includes a Phase 1 Clean Architecture API foundation for V.A.T Traders billing and inventory.

## Solution projects

- `VatTraders.Domain`: core entities (`Product`, `Customer`, `Invoice`, `InvoiceItem`) and domain enum (`GstCategory`).
- `VatTraders.Application`: DTOs, contracts (`IRepository`, `IInvoiceService`, `IUnitOfWork`, `IGstStrategy`) and billing business logic.
- `VatTraders.Infrastructure`: EF Core SQL Server `DbContext`, generic repository, transaction-capable unit of work.
- `VAT_Traders_Billing_Solution.Server`: ASP.NET Core Web API with controllers.

## API endpoints delivered in Phase 1

- `GET/POST/PUT/DELETE /api/products`
  - Enforces pesticide HSN code `3808`.
  - Returns low-stock and expiry flags from domain logic.
- `GET/POST /api/customers`
- `POST /api/invoices`
  - Applies GST Strategy pattern (`5%` bio-pesticide, `18%` standard).
  - Deducts stock and writes invoice in a single database transaction.

## Configuration

Set connection string in `VAT_Traders_Billing_Solution.Server/appsettings.json`:

```json
"ConnectionStrings": {
  "VatTradersDb": "Server=(localdb)\\MSSQLLocalDB;Database=VatTradersDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

The API calls `EnsureCreated` on startup for initial local development.

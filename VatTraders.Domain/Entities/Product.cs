using VatTraders.Domain.Enums;

namespace VatTraders.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string HsnCode { get; set; } = "3808";
    public GstCategory GstCategory { get; set; } = GstCategory.Standard;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int LowStockThreshold { get; set; } = 10;
    public DateOnly ExpiryDate { get; set; }
    public bool IsLowStock => StockQuantity <= LowStockThreshold;
    public bool IsExpired => ExpiryDate < DateOnly.FromDateTime(DateTime.UtcNow.Date);
}

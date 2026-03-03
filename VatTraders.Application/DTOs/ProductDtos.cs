using VatTraders.Domain.Enums;

namespace VatTraders.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string HsnCode,
    GstCategory GstCategory,
    decimal UnitPrice,
    int StockQuantity,
    DateOnly ExpiryDate,
    bool IsLowStock,
    bool IsExpired);

public record UpsertProductRequest(
    string Name,
    decimal UnitPrice,
    int StockQuantity,
    int LowStockThreshold,
    DateOnly ExpiryDate,
    GstCategory GstCategory,
    string HsnCode = "3808");

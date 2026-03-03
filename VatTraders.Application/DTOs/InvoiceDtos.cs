namespace VatTraders.Application.DTOs;

public record CreateInvoiceRequest(Guid CustomerId, IReadOnlyCollection<CreateInvoiceItemRequest> Items);

public record CreateInvoiceItemRequest(Guid ProductId, int Quantity);

public record InvoiceItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TaxRate, decimal TaxAmount, decimal LineTotal);

public record InvoiceDto(Guid Id, Guid CustomerId, DateTime IssuedAtUtc, decimal SubTotal, decimal GstTotal, decimal Cgst, decimal Sgst, decimal GrandTotal, IReadOnlyCollection<InvoiceItemDto> Items);

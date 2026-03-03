using VatTraders.Application.DTOs;

namespace VatTraders.Application.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default);
}

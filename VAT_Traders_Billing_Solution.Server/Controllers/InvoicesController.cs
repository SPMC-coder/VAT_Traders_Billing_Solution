using Microsoft.AspNetCore.Mvc;
using VatTraders.Application.DTOs;
using VatTraders.Application.Interfaces;

namespace VAT_Traders_Billing_Solution.Server.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var invoice = await _invoiceService.CreateInvoiceAsync(request, cancellationToken);
            return Ok(invoice);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }
}

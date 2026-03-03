using VatTraders.Application.DTOs;
using VatTraders.Application.Interfaces;
using VatTraders.Domain.Entities;

namespace VatTraders.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IGstStrategy _gstStrategy;
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceService(
        IRepository<Customer> customerRepository,
        IRepository<Product> productRepository,
        IRepository<Invoice> invoiceRepository,
        IGstStrategy gstStrategy,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _invoiceRepository = invoiceRepository;
        _gstStrategy = gstStrategy;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("At least one invoice item is required.");
        }

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new InvalidOperationException("Customer does not exist.");

        var invoice = new Invoice { CustomerId = customer.Id };

        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            foreach (var requestItem in request.Items)
            {
                if (requestItem.Quantity <= 0)
                {
                    throw new InvalidOperationException("Quantity must be greater than zero.");
                }

                var product = await _productRepository.GetByIdAsync(requestItem.ProductId, ct)
                    ?? throw new InvalidOperationException($"Product {requestItem.ProductId} not found.");

                if (product.IsExpired)
                {
                    throw new InvalidOperationException($"Product {product.Name} is expired and cannot be sold.");
                }

                if (product.StockQuantity < requestItem.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}.");
                }

                var rate = _gstStrategy.GetRate(product);
                var subTotal = requestItem.Quantity * product.UnitPrice;
                var taxAmount = Math.Round(subTotal * rate, 2, MidpointRounding.AwayFromZero);

                invoice.Items.Add(new InvoiceItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = requestItem.Quantity,
                    UnitPrice = product.UnitPrice,
                    TaxRate = rate,
                    TaxAmount = taxAmount,
                    LineTotal = subTotal + taxAmount
                });

                product.StockQuantity -= requestItem.Quantity;
                _productRepository.Update(product);
            }

            invoice.SubTotal = invoice.Items.Sum(i => i.Quantity * i.UnitPrice);
            invoice.GstTotal = invoice.Items.Sum(i => i.TaxAmount);
            invoice.Cgst = Math.Round(invoice.GstTotal / 2, 2, MidpointRounding.AwayFromZero);
            invoice.Sgst = invoice.GstTotal - invoice.Cgst;
            invoice.GrandTotal = invoice.SubTotal + invoice.GstTotal;

            await _invoiceRepository.AddAsync(invoice, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);

        return new InvoiceDto(
            invoice.Id,
            invoice.CustomerId,
            invoice.IssuedAtUtc,
            invoice.SubTotal,
            invoice.GstTotal,
            invoice.Cgst,
            invoice.Sgst,
            invoice.GrandTotal,
            invoice.Items.Select(i => new InvoiceItemDto(
                i.ProductId,
                i.Product?.Name ?? string.Empty,
                i.Quantity,
                i.UnitPrice,
                i.TaxRate,
                i.TaxAmount,
                i.LineTotal)).ToArray());
    }
}

using Microsoft.AspNetCore.Mvc;
using VatTraders.Application.DTOs;
using VatTraders.Application.Interfaces;
using VatTraders.Domain.Entities;

namespace VAT_Traders_Billing_Solution.Server.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> List(CancellationToken cancellationToken)
    {
        var products = await _productRepository.ListAsync(cancellationToken);
        return Ok(products.Select(Map).ToArray());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(Map(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] UpsertProductRequest request, CancellationToken cancellationToken)
    {
        if (!IsValidHsn(request.HsnCode))
        {
            return BadRequest("HSN code must be 3808 for pesticide inventory.");
        }

        var product = new Product
        {
            Name = request.Name,
            HsnCode = request.HsnCode,
            GstCategory = request.GstCategory,
            UnitPrice = request.UnitPrice,
            StockQuantity = request.StockQuantity,
            LowStockThreshold = request.LowStockThreshold,
            ExpiryDate = request.ExpiryDate
        };

        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, Map(product));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] UpsertProductRequest request, CancellationToken cancellationToken)
    {
        var existing = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        if (!IsValidHsn(request.HsnCode))
        {
            return BadRequest("HSN code must be 3808 for pesticide inventory.");
        }

        existing.Name = request.Name;
        existing.HsnCode = request.HsnCode;
        existing.GstCategory = request.GstCategory;
        existing.UnitPrice = request.UnitPrice;
        existing.StockQuantity = request.StockQuantity;
        existing.LowStockThreshold = request.LowStockThreshold;
        existing.ExpiryDate = request.ExpiryDate;

        _productRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(Map(existing));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        _productRepository.Remove(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static ProductDto Map(Product product) =>
        new(product.Id, product.Name, product.HsnCode, product.GstCategory, product.UnitPrice, product.StockQuantity, product.ExpiryDate, product.IsLowStock, product.IsExpired);

    private static bool IsValidHsn(string hsnCode) => hsnCode == "3808";
}

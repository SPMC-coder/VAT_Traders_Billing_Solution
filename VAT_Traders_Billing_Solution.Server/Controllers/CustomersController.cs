using Microsoft.AspNetCore.Mvc;
using VatTraders.Application.DTOs;
using VatTraders.Application.Interfaces;
using VatTraders.Domain.Entities;

namespace VAT_Traders_Billing_Solution.Server.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomersController(IRepository<Customer> customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CustomerDto>>> List(CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.ListAsync(cancellationToken);
        return Ok(customers.Select(c => new CustomerDto(c.Id, c.Name, c.Phone, c.Gstin)).ToArray());
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] UpsertCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer { Name = request.Name, Phone = request.Phone, Gstin = request.Gstin };
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Ok(new CustomerDto(customer.Id, customer.Name, customer.Phone, customer.Gstin));
    }
}

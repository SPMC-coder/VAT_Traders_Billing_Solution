namespace VatTraders.Application.DTOs;

public record CustomerDto(Guid Id, string Name, string? Phone, string? Gstin);

public record UpsertCustomerRequest(string Name, string? Phone, string? Gstin);

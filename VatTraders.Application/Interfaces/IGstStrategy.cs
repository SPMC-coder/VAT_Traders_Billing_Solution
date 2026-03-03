using VatTraders.Domain.Entities;

namespace VatTraders.Application.Interfaces;

public interface IGstStrategy
{
    decimal GetRate(Product product);
}

using VatTraders.Application.Interfaces;
using VatTraders.Domain.Entities;
using VatTraders.Domain.Enums;

namespace VatTraders.Application.Services;

public class GstRateStrategy : IGstStrategy
{
    public decimal GetRate(Product product)
    {
        return product.GstCategory == GstCategory.BioPesticide ? 0.05m : 0.18m;
    }
}

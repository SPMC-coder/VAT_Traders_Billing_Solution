using Microsoft.Extensions.DependencyInjection;
using VatTraders.Application.Interfaces;
using VatTraders.Application.Services;

namespace VatTraders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGstStrategy, GstRateStrategy>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        return services;
    }
}

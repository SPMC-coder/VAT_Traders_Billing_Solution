using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VatTraders.Application.Interfaces;
using VatTraders.Infrastructure.Data;
using VatTraders.Infrastructure.Repositories;

namespace VatTraders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("VatTradersDb")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=VatTradersDb;Trusted_Connection=True;TrustServerCertificate=True";

        services.AddDbContext<VatTradersDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

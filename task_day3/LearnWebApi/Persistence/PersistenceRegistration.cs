using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DatabaseContext;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        const string dbConnection = "Server=localhost;Database=boottest;Trusted_Connection=True;Encrypt=False;";

        services.AddDbContext<TableContext>(opt => opt.UseSqlServer(dbConnection));
        services.AddScoped<ITableSpecificationRepository, TableSpecificationRepository>();

        return services;
    }
}
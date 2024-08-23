using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DatabaseContext;
using Persistence.Repositories;
using StackExchange.Redis;

namespace Persistence;

public static class PersistenceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        const string dbConnection = "Server=localhost;Database=boottest;Trusted_Connection=True;Encrypt=False;";
        const string redisConnection = "localhost:6379";

        services.AddDbContext<TableContext>(opt => opt.UseSqlServer(dbConnection));
        services.AddScoped<ITableSpecificationRepository, TableSpecificationRepository>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "CacheInstance";
        });

        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(redisConnection));

        return services;
    }
}

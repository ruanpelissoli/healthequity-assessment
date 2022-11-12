using HealthEquity.Assessment.Domain.Repositories;
using HealthEquity.Assessment.Infrastructure.Repositories;
using HealthEquity.Assessment.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace HealthEquity.Assessment.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HealthEquityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("HealthEquityAssessmentContext")));

        services.AddScoped(options =>
        {
            var connectionString = configuration.GetSection("CacheSettings:ConnectionString").Value;

            if (connectionString == null)
                throw new InvalidOperationException("Redis database not initialized");

            var multiplexer = ConnectionMultiplexer.Connect(connectionString);

            return multiplexer.GetDatabase();
        });

        services.AddScoped<ICarsRepository, CarsRepository>();
        services.Decorate<ICarsRepository, CachedCarsRepository>();

        return services;
    }

    public static IServiceProvider SeedInfrastructureData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<HealthEquityDbContext>();
        context.Database.Migrate();

        SeedCars.Initialize(scope.ServiceProvider);

        return serviceProvider;
    }
}

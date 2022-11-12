using HealthEquity.Assessment.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HealthEquity.Assessment.Infrastructure.Seed;
internal static class SeedCars
{
    internal static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new HealthEquityDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<HealthEquityDbContext>>(),
            serviceProvider.GetRequiredService<IMediator>());

        if (context == null || context.Cars == null)
            throw new ArgumentNullException("Null database");

        if (context.Cars.Any()) return;

        context.Cars.AddRange(
            new Car("Audi", "R8", 2018, 2, "Red", 79995),
            new Car("Tesla", "3", 2018, 4, "Black", 54995),
            new Car("Porsche", "911 991", 2020, 2, "White", 155000),
            new Car("Mercedes-Benz", "GLE 63S", 2021, 5, "Blue", 83995),
            new Car("BMW", "X6 M", 2020, 5, "Silver", 62995)
        );

        context.SaveChanges();
    }
}

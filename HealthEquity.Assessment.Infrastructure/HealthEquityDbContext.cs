using HealthEquity.Assessment.Domain.Entities;
using HealthEquity.Assessment.Infrastructure.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthEquity.Assessment.Infrastructure;
internal class HealthEquityDbContext : DbContext
{
    private readonly IMediator _mediator;

    public HealthEquityDbContext(DbContextOptions<HealthEquityDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarConfiguration).Assembly);
    }

    public DbSet<Car> Cars { get; set; } = default!;

    public override int SaveChanges()
    {
        var response = base.SaveChanges();
        DispatchDomainEvents().GetAwaiter().GetResult();
        return response;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var response = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEvents();
        return response;
    }

    private async Task DispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<BaseEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
        }
    }
}

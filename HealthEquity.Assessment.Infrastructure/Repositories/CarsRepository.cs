using HealthEquity.Assessment.Domain.Entities;
using HealthEquity.Assessment.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthEquity.Assessment.Infrastructure.Repositories;
internal class CarsRepository : ICarsRepository
{
    private readonly HealthEquityDbContext _dbContext;

    public CarsRepository(HealthEquityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Car> AddAsync(Car entity)
    {
        var entry = await _dbContext.Cars.AddAsync(entity);

        return entry.Entity;
    }

    public void Delete(Car entity)
    {
        _dbContext.Cars.Remove(entity);
    }

    public async Task<Car?> GetByIdAsync(long id) =>
        await _dbContext.Cars.FirstOrDefaultAsync(car => car.Id == id);

    public async Task<IReadOnlyList<Car>> ListAllAsync() =>
        await _dbContext.Cars.ToListAsync();

    public void Update(Car entity)
    {
        _dbContext.Cars.Update(entity);
    }

    public async Task<Car> GetCarRandomly()
    {
        var rand = new Random();
        var skip = (int)(rand.NextDouble() * _dbContext.Cars.Count());

        return await _dbContext.Cars.OrderBy(o => o.Id).Skip(skip).Take(1).FirstAsync();
    }

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}

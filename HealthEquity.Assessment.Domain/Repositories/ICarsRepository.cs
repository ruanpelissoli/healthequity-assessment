using HealthEquity.Assessment.Domain.Entities;

namespace HealthEquity.Assessment.Domain.Repositories;
public interface ICarsRepository : IGenericRepository<Car>
{
    Task<Car> GetCarRandomly();
}

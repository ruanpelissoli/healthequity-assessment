using HealthEquity.Assessment.Domain.Entities;
using HealthEquity.Assessment.Domain.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace HealthEquity.Assessment.Infrastructure.Repositories;

internal class CachedCarsRepository : ICarsRepository
{
    private readonly ICarsRepository _decorated;
    private readonly IDatabase _redisDb;

    private const int _expiryInMinutes = 5;

    public CachedCarsRepository(ICarsRepository decorated, IDatabase redisDb)
    {
        _decorated = decorated;
        _redisDb = redisDb;
    }

    public async Task<Car> AddAsync(Car entity)
    {
        var fromDb = await _decorated.AddAsync(entity);

        await _redisDb.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(fromDb),
                TimeSpan.FromMinutes(_expiryInMinutes));

        return fromDb;
    }

    public Task Commit() => _decorated.Commit();

    public void Delete(Car entity)
    {
        _decorated.Delete(entity);

        _redisDb.StringGetDelete(entity.Id.ToString());
    }

    public async Task<Car?> GetByIdAsync(long id)
    {
        var cached = _redisDb.StringGet(id.ToString());

        if (cached.IsNull)
        {
            var fromDb = await _decorated.GetByIdAsync(id);

            if (fromDb == null) return fromDb;

            await _redisDb.StringSetAsync(id.ToString(), JsonSerializer.Serialize(fromDb),
                TimeSpan.FromMinutes(_expiryInMinutes));

            return fromDb;
        }

        return JsonSerializer.Deserialize<Car>(cached!);
    }

    public Task<Car> GetCarRandomly() => _decorated.GetCarRandomly();

    public async Task<IReadOnlyList<Car>> ListAllAsync() => await _decorated.ListAllAsync();

    public void Update(Car entity)
    {
        _decorated.Update(entity);
        _redisDb.StringGetDelete(entity.Id.ToString());

        _redisDb.StringSet(entity.Id.ToString(), JsonSerializer.Serialize(entity),
                TimeSpan.FromMinutes(_expiryInMinutes));
    }
}

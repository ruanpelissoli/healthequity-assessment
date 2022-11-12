using ErrorOr;
using HealthEquity.Assessment.Domain.Repositories;
using Mapster;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Queries.GetAllCars;

public sealed class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, ErrorOr<ICollection<CarDto>>>
{
    private readonly ICarsRepository _carsRepository;

    public GetAllCarsQueryHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<ErrorOr<ICollection<CarDto>>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
        var cars = await _carsRepository.ListAllAsync();

        return cars.Adapt<ICollection<CarDto>>().ToList();
    }
}

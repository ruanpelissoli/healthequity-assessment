using ErrorOr;
using HealthEquity.Assessment.Domain.Repositories;
using Mapster;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Queries.GetRandomCar;
public sealed class GetRandomCarQueryHandler : IRequestHandler<GetRandomCarQuery, ErrorOr<RandomCarDto>>
{
    private readonly ICarsRepository _carsRepository;

    public GetRandomCarQueryHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<ErrorOr<RandomCarDto>> Handle(GetRandomCarQuery request, CancellationToken cancellationToken)
    {
        var car = await _carsRepository.GetCarRandomly();

        if (car == null)
            return Error.NotFound("Car not found");

        return car.Adapt<RandomCarDto>();
    }
}

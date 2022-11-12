using ErrorOr;
using HealthEquity.Assessment.Domain.Repositories;
using Mapster;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Queries.GetCarDetail;
public sealed class GetCarDetailQueryHandler : IRequestHandler<GetCarDetailQuery, ErrorOr<CarDto>>
{
    private readonly ICarsRepository _carsRepository;

    public GetCarDetailQueryHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<ErrorOr<CarDto>> Handle(GetCarDetailQuery request, CancellationToken cancellationToken)
    {
        var car = await _carsRepository.GetByIdAsync(request.Id);

        if (car == null)
            return Error.NotFound("Car not found");

        return car.Adapt<CarDto>();
    }
}

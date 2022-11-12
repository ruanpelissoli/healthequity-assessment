using ErrorOr;
using HealthEquity.Assessment.Domain.Repositories;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.CheckGuessingPriceResult;

public sealed class CheckGuessingPriceResultCommandHandler : IRequestHandler<CheckGuessingPriceResultCommand, ErrorOr<bool>>
{
    private readonly ICarsRepository _carsRepository;

    public CheckGuessingPriceResultCommandHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<ErrorOr<bool>> Handle(CheckGuessingPriceResultCommand request, CancellationToken cancellationToken)
    {
        var car = await _carsRepository.GetByIdAsync(request.Id);

        if (car == null) return Error.NotFound();

        return request.Price >= car.Price - 5000 && request.Price <= car.Price + 5000;
    }
}

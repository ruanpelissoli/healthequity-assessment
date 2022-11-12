using ErrorOr;
using HealthEquity.Assessment.Domain.DomainEvents;
using HealthEquity.Assessment.Domain.Repositories;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.UpdateCar;

public sealed class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, ErrorOr<Unit>>
{
    private readonly ICarsRepository _carsRepository;

    public UpdateCarCommandHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
        var car = await _carsRepository.GetByIdAsync(request.Car.Id);

        if (car == null) return Error.NotFound();

        car.SetMake(request.Car.Make);
        car.SetModel(request.Car.Model);
        car.SetYear(request.Car.Year);
        car.SetDoors(request.Car.Doors);
        car.SetColor(request.Car.Color);
        car.SetPrice(request.Car.Price);

        _carsRepository.Update(car);
        car.QueueDomainEvent(new ShowSuccessMessageEvent($"Car model {car.Model} was updated"));
        await _carsRepository.Commit();

        return Unit.Value;
    }
}

using ErrorOr;
using HealthEquity.Assessment.Domain.DomainEvents;
using HealthEquity.Assessment.Domain.Entities;
using HealthEquity.Assessment.Domain.Repositories;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.CreateCar;

public sealed class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, ErrorOr<Unit>>
{
    private readonly ICarsRepository _carsRepository;

    public CreateCarCommandHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        var car = new Car(
            request.Car.Make,
            request.Car.Model,
            request.Car.Year,
            request.Car.Doors,
            request.Car.Color,
            request.Car.Price);

        await _carsRepository.AddAsync(car);
        car.QueueDomainEvent(new ShowSuccessMessageEvent($"Car model {car.Model} was created"));

        await _carsRepository.Commit();

        return Unit.Value;
    }
}

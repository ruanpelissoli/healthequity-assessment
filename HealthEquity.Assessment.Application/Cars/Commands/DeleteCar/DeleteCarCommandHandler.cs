using ErrorOr;
using HealthEquity.Assessment.Domain.DomainEvents;
using HealthEquity.Assessment.Domain.Repositories;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.DeleteCar;
public sealed class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, ErrorOr<Unit>>
{
    private readonly ICarsRepository _carsRepository;

    public DeleteCarCommandHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
    {
        var car = await _carsRepository.GetByIdAsync(request.Id);

        if (car == null) return Error.NotFound();

        _carsRepository.Delete(car);
        car.QueueDomainEvent(new ShowSuccessMessageEvent($"Car model {car.Model} was deleted"));
        await _carsRepository.Commit();

        return Unit.Value;
    }
}

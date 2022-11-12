using ErrorOr;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.UpdateCar;

public record UpdateCarCommand(CarDto Car) : IRequest<ErrorOr<Unit>>;

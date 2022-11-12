using ErrorOr;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.CreateCar;

public record CreateCarCommand(CarDto Car) : IRequest<ErrorOr<Unit>>;

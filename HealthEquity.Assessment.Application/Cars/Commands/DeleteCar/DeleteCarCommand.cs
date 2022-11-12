using ErrorOr;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.DeleteCar;

public record DeleteCarCommand(long Id) : IRequest<ErrorOr<Unit>>;
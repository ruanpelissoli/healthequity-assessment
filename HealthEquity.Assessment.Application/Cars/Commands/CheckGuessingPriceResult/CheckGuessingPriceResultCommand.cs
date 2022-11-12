using ErrorOr;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Commands.CheckGuessingPriceResult;
public record CheckGuessingPriceResultCommand(long Id, decimal Price) : IRequest<ErrorOr<bool>>;

using ErrorOr;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Queries.GetRandomCar;

public record GetRandomCarQuery() : IRequest<ErrorOr<RandomCarDto>>;

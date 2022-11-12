using ErrorOr;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Queries.GetAllCars;

public record GetAllCarsQuery() : IRequest<ErrorOr<ICollection<CarDto>>>
{
}

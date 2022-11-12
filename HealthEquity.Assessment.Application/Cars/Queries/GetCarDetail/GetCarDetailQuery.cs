using ErrorOr;
using MediatR;

namespace HealthEquity.Assessment.Application.Cars.Queries.GetCarDetail;

public record GetCarDetailQuery(long Id) : IRequest<ErrorOr<CarDto>>;
namespace HealthEquity.Assessment.Application.Cars;

public record CarDto(long Id, string Make, string Model, int Year, int Doors, string Color, decimal Price);
namespace HealthEquity.Assessment.Application.Cars;

public record RandomCarDto(long Id, string Make, string Model, int Year, int Doors, string Color);

public record GuessingPriceDto(RandomCarDto Car, decimal Price);

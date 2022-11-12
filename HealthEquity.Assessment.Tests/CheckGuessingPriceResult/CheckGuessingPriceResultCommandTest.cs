using Bogus;
using ErrorOr;
using FluentAssertions;
using HealthEquity.Assessment.Application.Cars.Commands.CheckGuessingPriceResult;
using HealthEquity.Assessment.Domain.Entities;
using HealthEquity.Assessment.Domain.Repositories;
using Moq;

namespace HealthEquity.Assessment.Tests.CheckGuessingPriceResult;
public class CheckGuessingPriceResultCommandTest
{
    private readonly Mock<ICarsRepository> _carsRepositoryMock;

    private readonly CheckGuessingPriceResultCommandHandler _handler;

    public CheckGuessingPriceResultCommandTest()
    {
        _carsRepositoryMock = new Mock<ICarsRepository>();

        _handler = new CheckGuessingPriceResultCommandHandler(_carsRepositoryMock.Object);
    }

    [Theory]
    [InlineData(5000)]
    [InlineData(10000)]
    [InlineData(15000)]
    public async Task Shoud_ValidateGuessingPrice_WhenIsCorrect(decimal guessingPrice)
    {
        // Arrange
        var price = 10000;

        var car = GenerateCar(price);

        var request = new CheckGuessingPriceResultCommand(car.Id, guessingPrice);

        _carsRepositoryMock.Setup(s => s.GetByIdAsync(car.Id))
            .ReturnsAsync(car);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Value.Should().BeTrue();
    }
    [Theory]
    [InlineData(4999)]
    [InlineData(15001)]
    public async Task Shoud_ValidateGuessingPrice_WhenIsIncorrect(decimal guessingPrice)
    {
        // Arrange
        var price = 10000;

        var car = GenerateCar(price);

        var request = new CheckGuessingPriceResultCommand(car.Id, guessingPrice);

        _carsRepositoryMock.Setup(s => s.GetByIdAsync(car.Id))
            .ReturnsAsync(car);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Value.Should().BeFalse();
    }

    [Fact]
    public async Task Should_ReturnErrorOrNotFound_WhenCarIsNull()
    {
        // Arrange
        var request = new CheckGuessingPriceResultCommand(It.IsAny<long>(), It.IsAny<decimal>());

        _carsRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Car?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.FirstError.Should().Be(Error.NotFound());
    }


    private static Car GenerateCar(decimal price) =>
        new Faker<Car>()
            .CustomInstantiator(f => new Car(
                f.Random.String2(20),
                f.Random.String2(20),
                f.Random.Int(1990, 2022),
                f.Random.Int(0, 5),
                f.Random.String2(10),
                price))
            .RuleFor(x => x.Id, f => f.Random.Long(1000, 99999))
            .Generate();
}

using Bogus;
using FluentAssertions;
using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Commands.CheckGuessingPriceResult;
using HealthEquity.Assessment.Domain.DomainEvents;
using HealthEquity.Assessment.Pages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HealthEquity.Assessment.Tests.Pages;
public class IndexPageTests
{
    private readonly Mock<IMediator> _mediatrMock;
    private readonly IndexModel _page;

    public IndexPageTests()
    {
        _mediatrMock = new Mock<IMediator>();

        _page = new IndexModel(_mediatrMock.Object);
    }

    [Fact]
    public async Task Should_ReturnRedirectToPageResultAndNotifySuccess_IfResultIsTrue()
    {
        var randomCar = GenerateFakeRandomCar();

        _page.GuessingPrice = new GuessingPriceDto(randomCar, It.IsAny<decimal>());

        _mediatrMock.Setup(s => s.Send(It.IsAny<CheckGuessingPriceResultCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _page.OnPostAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        _mediatrMock.Verify(f => f.Publish(It.IsAny<ShowSuccessMessageEvent>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Should_ReturnRedirectToPageResultAndNotifyError_IfResultIsFalse()
    {
        var randomCar = GenerateFakeRandomCar();

        _page.GuessingPrice = new GuessingPriceDto(randomCar, It.IsAny<decimal>());

        _mediatrMock.Setup(s => s.Send(It.IsAny<CheckGuessingPriceResultCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _page.OnPostAsync();

        result.Should().BeOfType<RedirectToPageResult>();
        _mediatrMock.Verify(f => f.Publish(It.IsAny<ShowErrorMessageEvent>(), It.IsAny<CancellationToken>()));
    }

    private static RandomCarDto GenerateFakeRandomCar() =>
        new Faker<RandomCarDto>()
            .CustomInstantiator(f => new RandomCarDto(
                f.Random.Long(1000, 99999),
                f.Random.String2(20),
                f.Random.String2(20),
                f.Random.Int(1990, 2022),
                f.Random.Int(0, 5),
                f.Random.String2(10)))
            .Generate();
}

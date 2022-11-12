using Bogus;
using ErrorOr;
using FluentAssertions;
using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Commands.CheckGuessingPriceResult;
using HealthEquity.Assessment.Application.Cars.Queries.GetRandomCar;
using HealthEquity.Assessment.Domain.DomainEvents;
using HealthEquity.Assessment.Pages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

    [Fact]
    public async Task Should_ReturnPageResult_IfModelStateIsInvalid()
    {
        _page.ModelState.AddModelError("error", "some error");

        var result = await _page.OnPostAsync();

        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task Should_ReturnPageResult_OnGetAsync()
    {
        var car = GenerateFakeRandomCar();

        _mediatrMock.Setup(s => s.Send(It.IsAny<GetRandomCarQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(car);

        var result = await _page.OnGetAsync();

        result.Should().BeOfType<PageResult>();
        _page.GuessingPrice.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_ReturnNotFoundResult_OnGetAsync()
    {
        var car = GenerateFakeRandomCar();

        _mediatrMock.Setup(s => s.Send(It.IsAny<GetRandomCarQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Error.NotFound());

        var result = await _page.OnGetAsync();

        result.Should().BeOfType<NotFoundObjectResult>();
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

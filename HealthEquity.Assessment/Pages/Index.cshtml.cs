using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Commands.CheckGuessingPriceResult;
using HealthEquity.Assessment.Application.Cars.Queries.GetRandomCar;
using HealthEquity.Assessment.Domain.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.Assessment.Pages;
public class IndexModel : PageModel
{
    private readonly IMediator _mediatr;

    public IndexModel(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [BindProperty]
    public GuessingPriceDto GuessingPrice { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await _mediatr.Send(new GetRandomCarQuery());

        return result.Match(
            car =>
            {
                GuessingPrice = new GuessingPriceDto(car, 0);
                return (IActionResult)Page();
            },
            error => NotFound(error));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _mediatr.Send(new CheckGuessingPriceResultCommand(GuessingPrice.Car.Id, GuessingPrice.Price));

        return result.Match(
            result =>
            {
                if (result)
                    _mediatr.Publish(new ShowSuccessMessageEvent("Nice shot!"));
                else
                    _mediatr.Publish(new ShowErrorMessageEvent("Not close, try again!"));

                return (IActionResult)RedirectToPage("./Index");
            },
            error => BadRequest());
    }
}

﻿using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Queries.GetCarDetail;
using HealthEquity.Assessment.Application.Cars.Queries.GetRandomCar;
using HealthEquity.Assessment.Domain.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.Assessment.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediatr;

    public IndexModel(ILogger<IndexModel> logger, IMediator mediatr)
    {
        _logger = logger;
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

        var result = await _mediatr.Send(new GetCarDetailQuery(GuessingPrice.Car.Id));

        return result.Match(
            car =>
            {
                if (GuessingPrice.Price >= car.Price - 5000 && GuessingPrice.Price <= car.Price + 5000)
                {
                    _mediatr.Publish(new ShowSuccessMessageEvent("Nice shot!"));
                }
                else
                {
                    _mediatr.Publish(new ShowErrorMessageEvent("Not close, try again!"));
                }

                return (IActionResult)RedirectToPage("./Index");
            },
            error => BadRequest());
    }
}

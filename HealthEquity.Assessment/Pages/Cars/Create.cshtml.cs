using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Commands.CreateCar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.Assessment.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly ISender _sender;

        public CreateModel(ISender sender)
        {
            _sender = sender;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CarDto Car { get; set; } = null!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _sender.Send(new CreateCarCommand(Car));

            return result.Match(
                ok =>
                    (IActionResult)RedirectToPage("./Index"),
                error => BadRequest());
        }
    }
}

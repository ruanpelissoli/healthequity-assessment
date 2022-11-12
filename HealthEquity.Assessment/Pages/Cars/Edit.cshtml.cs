using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Commands.UpdateCar;
using HealthEquity.Assessment.Application.Cars.Queries.GetCarDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.Assessment.Pages.Cars
{
    public class EditModel : PageModel
    {
        private readonly ISender _sender;

        public EditModel(ISender sender)
        {
            _sender = sender;
        }

        [BindProperty]
        public CarDto Car { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
                return NotFound();

            var result = await _sender.Send(new GetCarDetailQuery(id.Value));

            return result.Match(
                car =>
                {
                    Car = car;
                    return (IActionResult)Page();
                },
                error => NotFound(error));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _sender.Send(new UpdateCarCommand(Car));

            return result.Match(
                ok => (IActionResult)RedirectToPage("./Index"),
                error => NotFound(error));
        }
    }
}

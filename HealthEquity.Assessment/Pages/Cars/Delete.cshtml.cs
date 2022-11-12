using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Commands.DeleteCar;
using HealthEquity.Assessment.Application.Cars.Queries.GetCarDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.Assessment.Pages.Cars
{
    public class DeleteModel : PageModel
    {
        private readonly ISender _sender;

        public DeleteModel(ISender sender)
        {
            _sender = sender;
        }

        [BindProperty]
        public CarDto Car { get; set; } = null!;

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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
                return NotFound();

            var result = await _sender.Send(new DeleteCarCommand(id.Value));

            return result.Match(
                ok =>
                    (IActionResult)RedirectToPage("./Index"),
                error => BadRequest());
        }
    }
}

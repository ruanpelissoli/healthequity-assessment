using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Queries.GetCarDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.Assessment.Pages.Cars
{
    public class DetailsModel : PageModel
    {
        private readonly ISender _sender;

        public DetailsModel(ISender sender)
        {
            _sender = sender;
        }

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
    }
}

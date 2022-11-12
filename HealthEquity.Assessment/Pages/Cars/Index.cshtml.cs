using HealthEquity.Assessment.Application.Cars;
using HealthEquity.Assessment.Application.Cars.Queries.GetAllCars;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.Assessment.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly ISender _sender;

        public IndexModel(ISender sender)
        {
            _sender = sender;
        }

        public IList<CarDto> Cars { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _sender.Send(new GetAllCarsQuery());

            Cars = result.Match(
                cars => cars.ToList(),
                error => new List<CarDto>()
            );
        }
    }
}

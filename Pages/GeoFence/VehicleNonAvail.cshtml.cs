using GeoFence_VehicleNonAvailability.Models.Domain;
using GeoFence_VehicleNonAvailability.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeoFence_VehicleNonAvailability.Pages.GeoFence
{
    public class VehicleNonAvailModel : PageModel
    {

        private readonly VehicleNotAvailRepository vehicleNotAvailRepository;

        [BindProperty]
        public DateTime startDate { get; set; }
        public IEnumerable<VehicleNonAvailability> vehicleNonAvailabilities { get; set; }

        public VehicleNonAvailModel(VehicleNotAvailRepository vehicleNotAvailRepository)
        {
            this.vehicleNotAvailRepository = vehicleNotAvailRepository;
        }
        public void OnGet()
        {
        }
        public void OnPostDisplay()
        {
            vehicleNonAvailabilities = vehicleNotAvailRepository.vehicleNonAvailabilities(startDate);
        }
        public IActionResult OnPostClose()
        {
            return RedirectToPage("/Index");
        }
    }
}

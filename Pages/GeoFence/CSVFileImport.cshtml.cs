using GeoFence_VehicleNonAvailability.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CsvHelper;
using CsvHelper.Configuration;
using GeoFence_VehicleNonAvailability.Models.Domain;

namespace GeoFence_VehicleNonAvailability.Pages.GeoFence
{
    public class CSVFileImportModel : PageModel
    {
        private readonly GeoFencePeriodDBContext dbContext;

        public CSVFileImportModel(GeoFencePeriodDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostImport(IFormFile csvFile) 
        {
            if (csvFile == null || csvFile.Length <= 0)
            {
                ModelState.AddModelError("File", "Please select a valid CSV File");
                return Page();
            }

            var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(csvFile.OpenReadStream()))

            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<GeoFencePeriod>();

                foreach (var record in records)
                {
                    dbContext.GeoFencePeriods.Add(record);
                }
                dbContext.SaveChanges();
            }

            ViewData["Message"] = "CSV File Imported Successfully";

            return Page();
        }
        public IActionResult OnPostClose()
        {
            return RedirectToPage("/Index");
        }
    }
}

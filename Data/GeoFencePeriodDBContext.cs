using GeoFence_VehicleNonAvailability.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeoFence_VehicleNonAvailability.Data
{
    public class GeoFencePeriodDBContext : DbContext
    {
        public GeoFencePeriodDBContext(DbContextOptions<GeoFencePeriodDBContext> options) : base(options)
        {
        }
        public DbSet<GeoFencePeriod>  GeoFencePeriods { get; set;}

    }
}

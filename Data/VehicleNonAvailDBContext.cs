using GeoFence_VehicleNonAvailability.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeoFence_VehicleNonAvailability.Data
{
    public class VehicleNonAvailDBContext : DbContext
    {
        public VehicleNonAvailDBContext(DbContextOptions<VehicleNonAvailDBContext> options) : base(options)
        {
        }

        public DbSet<VehicleNonAvailability> vehicleNonAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use HasNoKey to specify it's a keyless entry
            modelBuilder.Entity<VehicleNonAvailability>().HasNoKey();
        }
    }
}

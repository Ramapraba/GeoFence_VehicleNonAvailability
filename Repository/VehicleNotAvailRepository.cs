using GeoFence_VehicleNonAvailability.Data;
using GeoFence_VehicleNonAvailability.Models.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GeoFence_VehicleNonAvailability.Repository
{
    public class VehicleNotAvailRepository
    {
        private readonly VehicleNonAvailDBContext dbContext;

        public VehicleNotAvailRepository(VehicleNonAvailDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<VehicleNonAvailability> vehicleNonAvailabilities(DateTime startDate)
        {
            return dbContext.vehicleNonAvailabilities.FromSqlRaw("EXEC GetVehicle_NonAvailable_HoursPerWeek @ip_MonthStartDate", new SqlParameter("@ip_MonthStartDate", startDate)).ToList();
        }
    }
}

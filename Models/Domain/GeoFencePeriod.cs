using System.ComponentModel.DataAnnotations.Schema;

namespace GeoFence_VehicleNonAvailability.Models.Domain
{
    public class GeoFencePeriod
    {
        public Guid ID { get; set; }
        public int vehicleid { get; set; }

        [Column(TypeName ="datetime2")]
        public DateTime entertime { get; set; }

        [Column(TypeName = "datetime2")] 
        public DateTime exittime { get; set; }
    }

}

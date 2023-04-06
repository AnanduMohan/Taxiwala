using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Taxiwala.Models.Constant;

namespace Taxiwala.Models
{
    public class Booking : BaseEntity
    {

        public string CustomerId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime PickupTime { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? Price { get; set; }
        public string? DriverId { get; set; }
        public bool IsConfirmed { get; set; }

    }
}

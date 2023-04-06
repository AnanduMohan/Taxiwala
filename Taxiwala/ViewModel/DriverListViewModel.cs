using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxiwala.ViewModel
{
    public class DriverListViewModel
    {
        public string CustomerName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime PickupTime { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? Price { get; set; }
        public string IsConfirmed { get; set; }
        public string  BookingId { get; set; }
    }
}

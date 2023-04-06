using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxiwala.ViewModel
{
    public class CustomerListViewModel
    {
        public CustomerListViewModel()
        {
            BillNumber = Guid.NewGuid().ToString();
        }
        public string CustomerName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime PickupTime { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? Price { get; set; }
        public string? DriverName { get; set; }
        public string IsConfirmed { get; set; }
        public string BookingId { get; set; }
        public string BillNumber { get; set; }
    }
}

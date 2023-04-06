using System.ComponentModel.DataAnnotations;

namespace Taxiwala.ViewModel
{
    public class BookingViewModel
    {
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public DateTime Pickup { get; set; }
    }
}

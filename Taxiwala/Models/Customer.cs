using System.ComponentModel.DataAnnotations;
using Taxiwala.Models.Constant;

namespace Taxiwala.Models
{
    public class Customer : BaseEntity
    {
        public Guid UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        

    }
}

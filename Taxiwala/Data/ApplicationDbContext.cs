using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Taxiwala.Models;

namespace Taxiwala.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        DbSet<Driver> Drivers { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Booking> Bookings { get; set; }
    }
}

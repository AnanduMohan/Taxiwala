using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Taxiwala.Data;
using Taxiwala.Models;
using Taxiwala.ViewModel;


namespace Taxiwala.Controllers
{
    public class BookingRideController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingRideController(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly RoleManager<IdentityRole> _roleManager;
        public async Task<IActionResult> Index(string id)
        {
            List<DriverListViewModel> driverListViewModels = new List<DriverListViewModel>();
            var booking = await _context.Set<Booking>().Where(x => x.Id.ToString() == id).FirstOrDefaultAsync();
            if (booking != null)
            {
                var customerName = await _context.Set<Customer>().Where(x => x.UserId.Equals(booking.CustomerId)).Select(p => $"{p.FirstName} {p.LastName}").FirstOrDefaultAsync();
                var drivename = await _context.Set<Customer>().Where(x => x.UserId.Equals(booking.DriverId)).Select(p => $"{p.FirstName} {p.LastName}").FirstOrDefaultAsync();

                DriverListViewModel driverList = new DriverListViewModel
                {
                    CustomerName = customerName,
                    Origin = booking.Origin,
                    Destination = booking.Destination,
                    PickupTime = booking.PickupTime,
                    Price = booking.Price,
                    BookingId = booking.Id.ToString(),
                    IsConfirmed = "Confirmed"

                };
                driverListViewModels.Add(driverList);
            }

            return View(driverListViewModels);
        }

        [HttpGet]
        public IActionResult CustomerBook()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DriverList(string id)
        {
           
           var booking = new List<Booking>();
            List<DriverListViewModel> driverListViewModels = new List<DriverListViewModel>();
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
            if (!id.IsNullOrEmpty())
            {
                booking = await _context.Set<Booking>().Where(x => x.Origin.ToLower().StartsWith(id.ToLower()) && x.IsConfirmed.Equals(false)).ToListAsync();

            }
            else
            {
                booking = await _context.Set<Booking>().Where(x => x.IsConfirmed.Equals(false)).ToListAsync();

            }

            foreach (var item in booking)
            {
                var customerName = await _context.Set<Customer>().Where(x => x.UserId.ToString().Equals(item.CustomerId)).Select(p => $"{p.FirstName} {p.LastName}").FirstOrDefaultAsync();
                DriverListViewModel driverList = new DriverListViewModel
                {
                    CustomerName = customerName,
                    Origin = item.Origin,
                    Destination = item.Destination,
                    PickupTime = item.PickupTime,
                    Price = item.Price,
                    BookingId = item.Id.ToString(),


                };
                driverListViewModels.Add(driverList);
            }
            ViewData["driverList"] = driverListViewModels;
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> DriverSearchList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomerBook(BookingViewModel bookingViewModel)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
            Booking book = new Booking
            {
                CustomerId = userId,
                Origin = bookingViewModel.From,
                Destination = bookingViewModel.To,
                PickupTime = bookingViewModel.Pickup

            };
            await _context.Set<Booking>().AddAsync(book);
            _context.SaveChanges();
            return RedirectToAction("customerlistview", "bookingride");
        }
        [HttpGet]
        public async Task<IActionResult> CustomerListView()
        {
            var customerMainList = new List<CustomerListViewModel>();
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
            var booking = await _context.Set<Booking>().Where(x => x.CustomerId.Equals(userId)).ToListAsync();
            foreach (var item in booking)
            {
                var customerName = await _context.Set<Customer>().Where(x => x.UserId.Equals(item.CustomerId)).Select(p => $"{p.FirstName} {p.LastName}").FirstOrDefaultAsync();
                var drivename = await _context.Set<Driver>().Where(x => x.UserId.ToString().Equals(item.DriverId)).Select(p => $"{p.FirstName} {p.LastName}").FirstOrDefaultAsync();
                CustomerListViewModel customerList = new CustomerListViewModel
                {
                    CustomerName =customerName,
                    DriverName = drivename,
                    Origin = item.Origin,
                    Destination = item.Destination,
                    PickupTime = item.PickupTime,
                    Price = item.Price,
                    IsConfirmed = item.IsConfirmed ? "Confirmed" : "Pending",
                    BookingId = item.Id.ToString()
                };
                customerMainList.Add(customerList);
            }
            ViewData["CustomerList"] = customerMainList;
            return View(customerMainList);
        }

        
        public async Task<IActionResult> DriverList(string bookingId, string price, SearchViewModel current)
        {
            if(!current.CurrentCity.IsNullOrEmpty())
            {
                return RedirectToAction("driverList", "BookingRide", new { id = current.CurrentCity });
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();


            var booking = await _context.Set<Booking>().Where(x => x.Id.ToString() == bookingId).FirstOrDefaultAsync();
            if (!userId.IsNullOrEmpty())
            {
                booking.DriverId = userId;
            }
            booking.IsConfirmed = true;
            booking.Price = Decimal.Parse(price);
            _context.Set<Booking>().Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "BookingRide", new { id = bookingId });
        }
    }
}


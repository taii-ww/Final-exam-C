using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MVC_Movie.Models;
using MVC_Movie.ViewModels;
using System;

namespace MVC_Movie.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController()
        {
            _context = new AppDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }

        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View(new HomeViewModel());

            // Nếu là admin → hiển thị tất cả rentals
            if (User.IsInRole(RoleName.CanManageMovies))
            {
                var allRentals = _context.Rentals
                    .Include("Movie")
                    .Include("Movie.Genre")
                    .Include("Customer")
                    .ToList();

                var now = DateTime.Now;

                var adminViewModel = new HomeViewModel
                {
                    CustomerName = "Admin",

                    // Thống kê
                    TotalCustomers = _context.Customers.Count(),
                    TotalMovies = _context.Movies.Count(),
                    TotalActiveRentals = allRentals.Count(r => r.DateReturned == null),
                    TotalOverdueRentals = allRentals.Count(r => r.DateReturned == null && r.ScheduledReturnDate.HasValue && r.ScheduledReturnDate.Value < now),
                    TotalRevenue = allRentals.Sum(r => r.PricePaid),
                    RevenueThisMonth = allRentals.Where(r => r.DateRented.Month == now.Month && r.DateRented.Year == now.Year).Sum(r => r.PricePaid),
                    TotalReviews = _context.Reviews.Count(),
                    ActivePromotions = _context.Promotions.Count(p => p.IsActive && p.StartDate <= now && p.EndDate >= now),

                    ActiveRentals = allRentals
                        .Where(r => r.DateReturned == null)
                        .Select(r => new RentalItem
                        {
                            RentalId = r.Id,
                            CustomerName = r.Customer.Name,
                            MovieTitle = r.Movie.Name,
                            Genre = r.Movie.Genre != null ? r.Movie.Genre.Name : "N/A",
                            DateRented = r.DateRented,
                            ScheduledReturnDate = r.ScheduledReturnDate,
                            DateReturned = r.DateReturned,
                            PricePaid = r.PricePaid
                        }).ToList(),

                    ReturnedRentals = allRentals
                        .Where(r => r.DateReturned != null)
                        .Select(r => new RentalItem
                        {
                            RentalId = r.Id,
                            CustomerName = r.Customer.Name,
                            MovieTitle = r.Movie.Name,
                            Genre = r.Movie.Genre != null ? r.Movie.Genre.Name : "N/A",
                            DateRented = r.DateRented,
                            ScheduledReturnDate = r.ScheduledReturnDate,
                            DateReturned = r.DateReturned,
                            PricePaid = r.PricePaid
                        }).ToList()
                };

                return View(adminViewModel);
            }

            // User thường → chỉ hiển thị rentals của mình
            var userId = User.Identity.GetUserId();
            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId);

            if (customer == null)
                return View(new HomeViewModel { CustomerName = User.Identity.GetUserName() });

            var rentals = _context.Rentals
                .Include("Movie")
                .Include("Movie.Genre")
                .Include("Customer")
                .Where(r => r.Customer.Id == customer.Id)
                .ToList();

            var viewModel = new HomeViewModel
            {
                CustomerName = customer.Name,
                ActiveRentals = rentals
                    .Where(r => r.DateReturned == null)
                    .Select(r => new RentalItem
                    {
                        RentalId = r.Id,
                        MovieTitle = r.Movie.Name,
                        Genre = r.Movie.Genre != null ? r.Movie.Genre.Name : "N/A",
                        DateRented = r.DateRented,
                        ScheduledReturnDate = r.ScheduledReturnDate,
                        DateReturned = r.DateReturned,
                         PricePaid = r.PricePaid
                    }).ToList(),

                ReturnedRentals = rentals
                    .Where(r => r.DateReturned != null)
                    .Select(r => new RentalItem
                    {
                        RentalId = r.Id,
                        MovieTitle = r.Movie.Name,
                        Genre = r.Movie.Genre != null ? r.Movie.Genre.Name : "N/A",
                        DateRented = r.DateRented,
                        ScheduledReturnDate = r.ScheduledReturnDate,
                        DateReturned = r.DateReturned,
                         PricePaid = r.PricePaid
                    }).ToList()
            };

            return View(viewModel);
        }


        [OutputCache(Duration = 50, Location = OutputCacheLocation.Server, VaryByParam = "genre")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
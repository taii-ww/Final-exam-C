using System;
using System.Linq;
using System.Web.Http;
using MVC_Movie.Models;

namespace MVC_Movie.Controllers.Api
{
    [Authorize(Roles = RoleName.CanManageMovies)]
    public class ActiveRentalsController : ApiController
    {
        private AppDbContext _context;
        public ActiveRentalsController() { _context = new AppDbContext(); }

        [HttpGet]
        public IHttpActionResult GetActiveRentals()
        {
            var rentals = _context.Rentals
                .Include("Customer")
                .Include("Movie")
                .Where(r => r.DateReturned == null)
                .ToList()
                .Select(r => new
                {
                    Id = r.Id,
                    CustomerName = r.Customer.Name,
                    MovieName = r.Movie.Name,
                    DateRented = r.DateRented.ToString("dd/MM/yyyy"),
                    ScheduledReturnDate = r.ScheduledReturnDate.HasValue
                        ? r.ScheduledReturnDate.Value.ToString("dd/MM/yyyy")
                        : "Chưa xác định",
                    PricePaid = r.PricePaid.ToString("N0") + " VNĐ",
                    IsOverdue = r.ScheduledReturnDate.HasValue
                        && r.ScheduledReturnDate.Value < DateTime.Now
                });

            return Ok(rentals);
        }
    }
}
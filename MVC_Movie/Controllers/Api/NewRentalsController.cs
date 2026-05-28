using System;
using System.Linq;
using System.Web.Http;
using MVC_Movie.Dtos;
using MVC_Movie.Models;

namespace MVC_Movie.Controllers.Api
{
    public class NewRentalsController : ApiController
    {
        private AppDbContext _appDbContext;

        public NewRentalsController()
        {
            _appDbContext = new AppDbContext();
        }

        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            var customer = _appDbContext.Customers.SingleOrDefault(
                c => c.Id == newRental.CustomerId);

            if (customer == null)
                return BadRequest("CustomerId is not valid.");

            if (newRental.MovieIds.Count == 0)
                return BadRequest("No Movie Ids have been given.");

            var movies = _appDbContext.Movies.Where(
                m => newRental.MovieIds.Contains(m.Id)).ToList();

            if (movies.Count != newRental.MovieIds.Count)
                return BadRequest("One or more MovieIds are invalid.");

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return BadRequest("Movie is not available");

                movie.NumberAvailable--;

                // Tính số ngày thuê, tối thiểu 1 ngày
                int rentalDays = (newRental.ScheduledReturnDate - newRental.ScheduledRentalDate).Days;
                if (rentalDays < 1) rentalDays = 1;

                var rental = new Rental()
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now,
                    ScheduledRentalDate = newRental.ScheduledRentalDate,
                    ScheduledReturnDate = newRental.ScheduledReturnDate,
                    PricePaid = movie.RentalPrice * rentalDays   // <-- Tính theo ngày
                };

                _appDbContext.Rentals.Add(rental);
            }

            _appDbContext.SaveChanges();

            return Ok();
        }
    }
}
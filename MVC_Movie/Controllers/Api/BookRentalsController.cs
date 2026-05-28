using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Http;
using MVC_Movie.Dtos;
using MVC_Movie.Models;

namespace MVC_Movie.Controllers.Api
{
    [Authorize]
    public class BookRentalsController : ApiController
    {
        private AppDbContext _appDbContext;

        public BookRentalsController()
        {
            _appDbContext = new AppDbContext();
        }

        [HttpPost]
        public IHttpActionResult BookRental(BookRentalDto bookRental)
        {
            var userId = User.Identity.GetUserId();
            var customer = _appDbContext.Customers
                .SingleOrDefault(c => c.UserId == userId);

            if (customer == null)
                return BadRequest(new { message = "No customer profile found for this user." }.ToString());

            var movie = _appDbContext.Movies
                .SingleOrDefault(m => m.Id == bookRental.MovieId);

            if (movie == null)
                return BadRequest(new { message = "Movie not found." }.ToString());

            if (movie.NumberAvailable == 0)
                return BadRequest(new { message = "Movie is not available." }.ToString());

            if (bookRental.ScheduledReturnDate <= bookRental.ScheduledRentalDate)
                return BadRequest(new { message = "Return date must be after rental date." }.ToString());

            var membershipType = _appDbContext.MembershipTypes
    .SingleOrDefault(m => m.Id == customer.MembershipTypeId);

            var discount = membershipType != null ? membershipType.DiscountRate : 0;
            // Kiểm tra mã khuyến mãi
            var promoDiscount = 0;
            if (!string.IsNullOrEmpty(bookRental.PromoCode))
            {
                var promo = _appDbContext.Promotions.SingleOrDefault(p =>
                    p.Code == bookRental.PromoCode &&
                    p.IsActive &&
                    p.StartDate <= DateTime.Now &&
                    p.EndDate >= DateTime.Now &&
                    (p.MovieId == null || p.MovieId == bookRental.MovieId));  // null = tất cả phim
                if (promo != null)
                    promoDiscount = promo.DiscountPercent;
            }

            int rentalDays = (bookRental.ScheduledReturnDate - bookRental.ScheduledRentalDate).Days;
            if (rentalDays < 1) rentalDays = 1;

            var pricePaid = movie.RentalPrice
                * rentalDays
                * (1 - (decimal)discount / 100)
                * (1 - (decimal)promoDiscount / 100);

            movie.NumberAvailable--;

            var rental = new Rental
            {
                Customer = customer,
                Movie = movie,
                DateRented = DateTime.Now,
                ScheduledRentalDate = bookRental.ScheduledRentalDate,
                ScheduledReturnDate = bookRental.ScheduledReturnDate,
                PricePaid = pricePaid
            };

            _appDbContext.Rentals.Add(rental);
            _appDbContext.SaveChanges();

            return Ok();
        }
    }
}
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using MVC_Movie.Models;

namespace MVC_Movie.Controllers.Api
{
    [Authorize]
    public class ReviewsController : ApiController
    {
        private AppDbContext _context;
        public ReviewsController() { _context = new AppDbContext(); }

        [HttpPost]
        public IHttpActionResult CreateReview(Review review)
        {
            var userId = User.Identity.GetUserId();
            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId);
            if (customer == null)
                return BadRequest("Tài khoản chưa liên kết khách hàng.");

            // Kiểm tra đã thuê phim chưa
            var hasRented = _context.Rentals
                .Any(r => r.Customer.Id == customer.Id && r.Movie.Id == review.MovieId);
            if (!hasRented)
                return BadRequest("Bạn chưa thuê phim này.");

            // Kiểm tra đã review chưa
            var alreadyReviewed = _context.Reviews
    .Any(r => r.CustomerId == customer.Id && r.MovieId == review.MovieId);
            if (alreadyReviewed)
                return BadRequest("Bạn đã đánh giá phim này rồi.");

            var newReview = new Review
            {
                CustomerId = customer.Id,
                MovieId = review.MovieId,
                Rating = review.Rating,
                Comment = review.Comment
            };

            _context.Reviews.Add(newReview);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteReview(int id)
        {
            var userId = User.Identity.GetUserId();
            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId);
            var review = _context.Reviews
                .SingleOrDefault(r => r.Id == id && r.CustomerId == customer.Id);
            if (review == null)
                return NotFound();
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return Ok();
        }
    }
}
using System;
using System.Linq;
using System.Web.Http;
using MVC_Movie.Models;

namespace MVC_Movie.Controllers.Api
{
    [Authorize(Roles = RoleName.CanManageMovies)]
    public class ReturnRentalsController : ApiController
    {
        private AppDbContext _context;
        public ReturnRentalsController() { _context = new AppDbContext(); }

        [HttpPost]
        public IHttpActionResult ReturnRental([FromBody] int rentalId)
        {
            var rental = _context.Rentals
                .Include("Movie")
                .SingleOrDefault(r => r.Id == rentalId);

            if (rental == null)
                return NotFound();

            if (rental.DateReturned != null)
                return BadRequest("Phim này đã được trả rồi.");

            rental.DateReturned = DateTime.Now;
            rental.Movie.NumberAvailable++;
            _context.SaveChanges();

            return Ok();
        }
    }
}
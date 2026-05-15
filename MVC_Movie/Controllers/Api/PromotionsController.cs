using System;
using System.Linq;
using System.Web.Http;
using MVC_Movie.Models;

namespace MVC_Movie.Controllers.Api
{
    public class PromotionsApiController : ApiController
    {
        private AppDbContext _context;
        public PromotionsApiController() { _context = new AppDbContext(); }

        [HttpGet]
        [Route("api/promotions/validate")]
        public IHttpActionResult ValidateCode(string code)
        {
            var promo = _context.Promotions.SingleOrDefault(p =>
                p.Code == code &&
                p.IsActive &&
                p.StartDate <= DateTime.Now &&
                p.EndDate >= DateTime.Now);

            if (promo == null)
                return NotFound();

            return Ok(new { discountPercent = promo.DiscountPercent });
        }

        [HttpGet]
        [Route("api/promotions/validate")]
        public IHttpActionResult ValidateCode(string code, int movieId)
        {
            var promo = _context.Promotions.SingleOrDefault(p =>
                p.Code == code &&
                p.IsActive &&
                p.StartDate <= DateTime.Now &&
                p.EndDate >= DateTime.Now &&
                (p.MovieId == null || p.MovieId == movieId));

            if (promo == null)
                return NotFound();

            return Ok(new { discountPercent = promo.DiscountPercent });
        }
    }
}
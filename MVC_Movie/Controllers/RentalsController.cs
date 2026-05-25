using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC_Movie.Models; // Thêm dòng này

namespace MVC_Movie.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            return View();
        }


        public ActionResult Book(int id)
        {
            var context = new AppDbContext();
            var movie = context.Movies.SingleOrDefault(m => m.Id == id);
            if (movie == null)
                return HttpNotFound();

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var customer = context.Customers
                .Include("MembershipType")
                .SingleOrDefault(c => c.UserId == userId);

            var discount = customer?.MembershipType?.DiscountRate ?? 0;
            var finalPrice = movie.RentalPrice * (1 - (decimal)discount / 100);

            ViewBag.MovieId = id;
            ViewBag.MovieName = movie.Name;
            ViewBag.RentalPrice = movie.RentalPrice;
            ViewBag.Discount = discount;
            ViewBag.FinalPrice = finalPrice;

            return View();
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Return()
        {
            return View();
        }
    }
}
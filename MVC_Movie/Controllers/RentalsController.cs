using System.Linq;
using System.Web.Mvc;
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

            ViewBag.MovieId = id;
            ViewBag.MovieName = movie.Name;
            return View();
        }
    }
}
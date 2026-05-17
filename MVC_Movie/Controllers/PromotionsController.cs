using System.Linq;
using System.Web.Mvc;
using MVC_Movie.Models;

namespace MVC_Movie.Controllers
{
    public class PromotionsController : Controller
    {
        private AppDbContext _context;
        public PromotionsController() { _context = new AppDbContext(); }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Index()
        {
            var promotions = _context.Promotions.ToList();
            return View(promotions);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            ViewBag.Movies = _context.Movies.ToList();
            return View("PromotionForm", new Promotion());
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        [HttpPost]
        public ActionResult Save(Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Movies = _context.Movies.ToList();
                return View("PromotionForm", promotion);
            }
            if (promotion.Id == 0)
                _context.Promotions.Add(promotion);
            else
            {
                var promoInDb = _context.Promotions.Single(p => p.Id == promotion.Id);
                promoInDb.Code = promotion.Code;
                promoInDb.DiscountPercent = promotion.DiscountPercent;
                promoInDb.StartDate = promotion.StartDate;
                promoInDb.EndDate = promotion.EndDate;
                promoInDb.IsActive = promotion.IsActive;
                promoInDb.MovieId = promotion.MovieId;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var promo = _context.Promotions.SingleOrDefault(p => p.Id == id);
            if (promo == null)
                return HttpNotFound();
            ViewBag.Movies = _context.Movies.ToList();
            return View("PromotionForm", promo);
        }
    }
}
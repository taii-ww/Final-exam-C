using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MVC_Movie.Models;
using MVC_Movie.ViewModels;

namespace MVC_Movie.Controllers
{
    [Authorize]  // Thay [AllowAnonymous] thành [Authorize]
    public class CustomersController : Controller
    {
        private AppDbContext _appDbContext;

        public CustomersController()
        {
            _appDbContext = new AppDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _appDbContext.Dispose();
        }

        [AllowAnonymous]  // Ai cũng xem được danh sách
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var customer = _appDbContext.Customers.Include(c => c.MembershipType).Single(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();
            return View(customer);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var membershipTypes = _appDbContext.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel()
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _appDbContext.MembershipTypes.ToList()
                };
                return View("CustomerForm", viewModel);
            }
            if (customer.Id == 0)
                _appDbContext.Customers.Add(customer);
            else
            {
                var customerInDb = _appDbContext.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }
            _appDbContext.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var customer = _appDbContext.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();
            var viewModel = new CustomerFormViewModel()
            {
                Customer = customer,
                MembershipTypes = _appDbContext.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }
    }
}
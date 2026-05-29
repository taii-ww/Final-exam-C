using MVC_Movie.Models;

namespace MVC_Movie.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _db;

        public CustomerService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Thêm customer mới vào DB.
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            _db.Customers.Add(customer);
            _db.SaveChanges();
        }

        /// <summary>
        /// Cập nhật thông tin customer đã có trong DB.
        /// Trả về false nếu không tìm thấy.
        /// </summary>
        public bool UpdateCustomer(Customer customer)
        {
            var customerInDb = _db.Customers.Find(customer.Id);
            if (customerInDb == null)
                return false;

            customerInDb.Name = customer.Name;
            customerInDb.Birthdate = customer.Birthdate;
            customerInDb.MembershipTypeId = customer.MembershipTypeId;
            customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;

            _db.SaveChanges();
            return true;
        }
    }
}
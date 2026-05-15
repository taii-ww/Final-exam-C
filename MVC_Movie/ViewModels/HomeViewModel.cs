using System;
using System.Collections.Generic;

namespace MVC_Movie.ViewModels
{
    public class HomeViewModel
    {
        public string CustomerName { get; set; }
        public List<RentalItem> ActiveRentals { get; set; }
        public List<RentalItem> ReturnedRentals { get; set; }

        // Thống kê admin
        public int TotalCustomers { get; set; }
        public int TotalMovies { get; set; }
        public int TotalActiveRentals { get; set; }
        public int TotalOverdueRentals { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public int TotalReviews { get; set; }
        public int ActivePromotions { get; set; }
    }

    public class RentalItem
    {
        public int RentalId { get; set; }
        public string CustomerName { get; set; }
        public string MovieTitle { get; set; }
        public string Genre { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? ScheduledReturnDate { get; set; }
        public DateTime? DateReturned { get; set; }
        public decimal PricePaid { get; set; }
        public bool IsOverdue => DateReturned == null && ScheduledReturnDate.HasValue && ScheduledReturnDate.Value < DateTime.Now;
    }
}
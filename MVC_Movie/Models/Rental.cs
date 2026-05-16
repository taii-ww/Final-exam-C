using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_Movie.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; }

        [Required]
        public Movie Movie { get; set; }

        public DateTime DateRented { get; set; }

        public DateTime? DateReturned { get; set; }

        public DateTime? ScheduledRentalDate { get; set; }
        public DateTime? ScheduledReturnDate { get; set; }

        public decimal PricePaid { get; set; }
    }
}
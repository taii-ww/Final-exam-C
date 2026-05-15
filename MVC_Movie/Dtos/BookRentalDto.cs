using System;

namespace MVC_Movie.Dtos
{
    public class BookRentalDto
    {
        public int MovieId { get; set; }
        public DateTime ScheduledRentalDate { get; set; }
        public DateTime ScheduledReturnDate { get; set; }

        public string PromoCode { get; set; }
    }
}
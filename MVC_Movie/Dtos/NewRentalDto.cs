using System;
using System.Collections.Generic;

namespace MVC_Movie.Dtos
{
    public class NewRentalDto
    {
        public int CustomerId { get; set; }
        public List<int> MovieIds { get; set; }
        public DateTime ScheduledRentalDate { get; set; }
        public DateTime ScheduledReturnDate { get; set; }
    }
}
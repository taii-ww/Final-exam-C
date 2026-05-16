using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_Movie.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [Range(1, 100)]
        public byte DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public int? MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
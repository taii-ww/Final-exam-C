using System.ComponentModel.DataAnnotations;

namespace MVC_Movie.Models
{
    public class Review
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
        [Range(1, 5)]
        public byte Rating { get; set; }
        [StringLength(500)]
        public string Comment { get; set; }
    }
}
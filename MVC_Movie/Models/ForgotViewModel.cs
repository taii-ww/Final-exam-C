using System.ComponentModel.DataAnnotations;

namespace MVC_Movie.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
namespace MVC_Movie.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter customer's name.")]
        [StringLength(255)]
        public string Name { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? Birthdate { get; set; }
        public bool IsSubscribedToNewsletter { get; set; }
        public MembershipType MembershipType { get; set; }
        [Display(Name = "Membership Type")]
        [Min18YearsIfAMember]
        public byte MembershipTypeId { get; set; }

        // Thêm dòng này
        public string UserId { get; set; }

        public string Email { get; set; }
    }
}
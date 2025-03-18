using FirstProjectNET.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace FirstProjectNET.Models.ViewModel
{
    public class CustomerViewModel
    {
        [Display(Name ="Customer ID")]
        public string CustomerID { get; set; }

        [Display(Name = "Account ID")]
        public string AccountID { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

                
        
        [Required(ErrorMessage = "Firstname is not Null")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Name can only contain letters.")]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Firstname is not Null")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Name can only contain letters.")]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@gmail\.com",
            ErrorMessage = "Email must be entered in the format ...gmail.com")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone is not null!")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number can only contain digits.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 digits.")]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Display(Name ="Address")]
        public string? Address { get; set; }
        [Display(Name ="Membership")]

        public Membership Membership { get; set; }
    }
}

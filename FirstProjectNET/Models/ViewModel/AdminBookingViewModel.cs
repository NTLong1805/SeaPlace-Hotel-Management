using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FirstProjectNET.Models.ViewModel
{
    public class AdminBookingViewModel
    {
        public string BookingID { get; set; }

        [Required(ErrorMessage = "CustomerID is required")]
        public string CustomerID { get; set; }

        [Required(ErrorMessage = "DateCome is required")]
        [DataType(DataType.Date)]
        public DateTime DateCome { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "DateGo is required")]
        [DataType(DataType.Date)]
        public DateTime DateGo { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Number of people must be at least 1")]
        public int NumberPeople { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Deposit must be a positive number")]
        public decimal Deposit { get; set; }

        public bool Status { get; set; }

        public List<SelectListItem> CustomerList { get; set; }
    }

}

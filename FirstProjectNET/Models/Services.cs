using System.ComponentModel.DataAnnotations;

namespace FirstProjectNET.Models
{
	public class Services
	{
		public Services()
		{
			RoomServices = new HashSet<RoomService>();
		}

		[Key]
		public string ServiceID { get; set; }

		[Required(ErrorMessage ="Service Name is required")]
		public string? ServiceName { get; set; }

		[Required(ErrorMessage ="Price is required")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Description is required")]
		public string Description { get; set; }


		public virtual ICollection<RoomService> RoomServices { get; set; }
	}
}

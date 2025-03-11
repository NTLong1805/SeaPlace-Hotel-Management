using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FirstProjectNET.Models
{
	public class RoomService
	{
		[Key]
		public int RoomServiceID { get; set; }

		public string? RoomID { get; set; }
		public virtual Room? Room { get; set; }
		public string? ServiceID { get; set; }
		public virtual Services? Service { get; set; }
		
	}
}

using System.Runtime.CompilerServices;

namespace FirstProjectNET.Models.ViewModel
{
    public class PropertiesViewModel
    {
        public class CategoryViewModel
        {
            public string CategoryID { get; set; }
            public string TypeName { get; set; }
            public decimal Capacity {  get; set; }
            
            public decimal Price { get; set; }
        }
        public class RoomAvailableViewModel
        {
            public string RoomID { get; set; }
            public CategoryViewModel Category { get; set; }
            public string Status { get; set; }           
            public List<string> ImageUrl {  get; set; }
            
        }

        public DateTime DateCome { get; set; }
        public DateTime DateGo { get; set; }
        public int NumberPeople { get; set; }

        public List<string> ServiceName { get; set; }



        public class DetailRoomViewModel
        {
            public string CategoryID { get; set; }
            public string TypeName { get; set; }
            public decimal Capacity { get; set; }

            public decimal Price { get; set; }

            public string ImageUrl { get; set; }

            public List<string> ServiceName { get; set; }
        }

        public class RateViewModel
        {
            public string RateID { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }

            public decimal Point {  get; set; }
            public string Message {  get; set; }
            public DateTime DateCreate { get; set; }

        }



        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<RoomAvailableViewModel> RoomAvailableViewModels { get; set; }

        public IEnumerable<DetailRoomViewModel> DetailRoomViewModels { get; set; }

        public IEnumerable<RateViewModel> RateViewModels { get; set; }
    }
}

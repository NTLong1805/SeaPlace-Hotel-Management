namespace FirstProjectNET.Models.ViewModel
{
    public class HomeViewModel
    {
        public class ServiceViewModel
        {
            public string Description { get; set; }

            public string ServiceName { get; set; }
        }

        public class RateViewModel
        {
            public string Username { get; set; }
            public DateTime DateCreate { get; set; }
            public double Point { get; set; }
            public string Message { get; set; }
        }
        public class CategoryViewModel
        {
            public string CategoryID {  get; set; }
            public string TypeName { get; set; }
            public int Capacity {  get; set; }
            public decimal Price {  get; set; }
        }

        public IEnumerable<ServiceViewModel> Services { get; set; } 

        public IEnumerable<RateViewModel> Rates { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}

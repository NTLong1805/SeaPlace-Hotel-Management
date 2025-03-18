namespace FirstProjectNET.Models.ViewModel
{
    public class AdminCategoryViewModel
    {
        public string CategoryID { get; set; }

        public string TypeName { get; set; }

        public decimal Price { get; set; }

        public decimal Capacity { get; set; }

        public List<string> ListTypeName { get; set; } 
    }
}

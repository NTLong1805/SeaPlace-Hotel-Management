namespace FirstProjectNET.Models
{
    public class AccountLogin
    {
        public int AccountID { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public Account Account { get; set; }
    }
}

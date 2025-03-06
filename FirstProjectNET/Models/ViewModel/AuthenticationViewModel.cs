using System.ComponentModel.DataAnnotations;

namespace FirstProjectNET.Models.ViewModel
{
	public class AuthenticationViewModel
	{
		[Required]
		[EmailAddress]
		public string SignInEmail {  get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string SignInPassword { get; set; }
		[Display(Name ="Remember Me")]
		public bool RememberMe {  get; set; }

		[Required]
		[EmailAddress]
		public string SignUpEmail   {  get; set; }
		[Required(ErrorMessage = "Username is required")]
		public string SignUpUserName {  get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string SignUpPassword {  get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare("SignUpPassword",ErrorMessage ="Password do not match")]
		public string SignUpConfirmPassword {  get; set; }

		public bool IsRegister {  get; set; }


	}
}

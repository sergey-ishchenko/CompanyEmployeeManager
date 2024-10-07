using System.ComponentModel.DataAnnotations;

namespace CompanyEmployeeManager.Models.Auth
{
	public class RegisterModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }
	}
}

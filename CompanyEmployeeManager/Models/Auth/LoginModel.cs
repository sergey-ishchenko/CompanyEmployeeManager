using System.ComponentModel.DataAnnotations;

namespace CompanyEmployeeManager.Models.Auth
{
	public class LoginModel
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
	}
}

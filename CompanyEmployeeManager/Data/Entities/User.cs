namespace CompanyEmployeeManager.Data.Entities
{
	public class User : BaseEntity
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public DateTime LastActivity { get; set; }
	}
}

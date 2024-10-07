namespace CompanyEmployeeManager.Data.Entities
{
	public class Experience : BaseEntity
	{
		public int EmployeeId { get; set; }
		public int LanguageId { get; set; }

		public Employee? Employee { get; set; }
		public Language? Language { get; set; }
	}
}

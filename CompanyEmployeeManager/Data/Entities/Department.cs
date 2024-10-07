namespace CompanyEmployeeManager.Data.Entities
{
	public class Department : BaseEntity
	{
		public string Name { get; set; } = "";
		public Byte Floor { get; set; }

		public IEnumerable<Employee> Users { get; set; } = null!;

	}
}

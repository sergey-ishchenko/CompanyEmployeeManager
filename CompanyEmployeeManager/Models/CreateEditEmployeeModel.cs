namespace CompanyEmployeeManager.Models
{
	public class CreateEditEmployeeModel
	{
		public EmployeeDto Employee { get; set; }
		public IEnumerable<int> Languages { get; set; }
	}
}

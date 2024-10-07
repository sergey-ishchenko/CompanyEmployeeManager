using CompanyEmployeeManager.Enums;

namespace CompanyEmployeeManager.Data.Entities;

public class Employee : BaseEntity
{
	public string Name { get; set; } = "";
	public string Surname { get; set; } = "";
	public byte Age { get; set; }
	public GenderEnum Gender { get; set; }
	public int DepartmentId { get; set; }
	public bool IsDeleted { get; set; }

	public Department? Department { get; set; }
	public IEnumerable<Experience> Experiences { get; set; } = null!;
}
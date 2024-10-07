using CompanyEmployeeManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace CompanyEmployeeManager.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
		[Required]
		public string Name { get; set; } = "";
		[Required]
		public string Surname { get; set; } = "";
        public byte Age { get; set; }
        public GenderEnum Gender { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDto? Department { get; set; }

    }
}

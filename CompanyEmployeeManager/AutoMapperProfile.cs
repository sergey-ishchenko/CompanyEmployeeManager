using AutoMapper;
using CompanyEmployeeManager.Data.Entities;
using CompanyEmployeeManager.Models;

namespace CompanyEmployeeManager
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Employee, EmployeeDto>().ReverseMap();
			CreateMap<Department, DepartmentDto>().ReverseMap();
			CreateMap<Department, DepartmentNamesDto>();
			CreateMap<Language, LanguageDto>();
		}
	}
}

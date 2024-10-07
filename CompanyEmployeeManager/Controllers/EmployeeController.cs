using AutoMapper;
using CompanyEmployeeManager.Data;
using CompanyEmployeeManager.Data.Entities;
using CompanyEmployeeManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployeeManager.Controllers;

[Authorize]
public class EmployeeController : Controller
{
	private readonly ApplicationDbContext _db;
	private readonly IMapper _mapper;

	public EmployeeController(ApplicationDbContext db,
		IMapper mapper)
	{
		_db = db;
		_mapper = mapper;
	}

	[AllowAnonymous]
	public IActionResult Index()
	{
		if (!HttpContext.User.Identity?.IsAuthenticated ?? false)
		{
			return Redirect("/Home/Index");
		}

		var employeesList = _db.Employees
			.Include(employee => employee.Department)
			.AsNoTracking()
			.AsQueryable();
		var employeeListDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesList);

		return View(new EmployeeListModel { Employees = employeeListDto });
	}

	[HttpGet("add")]
	public IActionResult Add()
	{
		ViewBag.Departments = GetDepartmentSelectList();
		ViewBag.Languages = GetLanguagesList();

		return View();
	}

	[HttpPost("add")]
	public async Task<IActionResult> Add(CreateEditEmployeeModel model)
	{
		if (ModelState.IsValid)
		{
			var employee = _mapper.Map<Employee>(model.Employee);
			employee.Experiences =
				model.Languages.Select(l => new Experience() { LanguageId = l }).ToList();

			_db.Employees.Add(employee);
			await _db.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		ViewBag.Departments = GetDepartmentSelectList();
		ViewBag.Languages = GetLanguagesList();

		return View(model);
	}

	[HttpGet("edit")]
	public IActionResult Edit(int employeeId)
	{
		var employee = _db.Employees
			.Include(emp => emp.Experiences)
			.FirstOrDefault(eml => eml.Id == employeeId);

		if (employee == null)
			return NotFound();

		ViewBag.Departments = GetDepartmentSelectList();
		ViewBag.Languages = GetLanguagesList();

		var model = new CreateEditEmployeeModel()
		{
			Employee = _mapper.Map<EmployeeDto>(employee),
			Languages = employee.Experiences.Select(exp => exp.LanguageId)
		};

		return View(model);
	}

	[HttpPost("edit")]
	public async Task<IActionResult> Edit(CreateEditEmployeeModel model)
	{
		if (ModelState.IsValid)
		{
			var employeeDb = _db.Employees
				.Include(emp => emp.Experiences)
				.SingleOrDefault(e => e.Id == model.Employee.Id);

			if (employeeDb != null)
			{
				var editedEmployee = _mapper.Map(model.Employee, employeeDb);
				editedEmployee.Experiences = model.Languages.Select(l => new Experience() { LanguageId = l }).ToList();

				await _db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
		}

		ViewBag.Departments = GetDepartmentSelectList();
		ViewBag.Languages = GetLanguagesList();

		return View(model);
	}

	[HttpGet("delete")]
	public IActionResult Delete(int employeeId)
	{
		var employee = _db.Employees.FirstOrDefault(eml => eml.Id == employeeId);
		if (employee == null)
			return NotFound();

		return View(_mapper.Map<EmployeeDto>(employee));
	}

	[HttpPost("delete")]
	public async Task<IActionResult> Delete_(int employeeId)
	{
		if (ModelState.IsValid)
		{
			var employee = _db.Employees.FirstOrDefault(eml => eml.Id == employeeId);
			if (employee == null)
				return NotFound();

			employee.IsDeleted = true;
			_db.Employees.Update(employee);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		return View();
	}

	[HttpPost]
	public JsonResult GetEmployeesNames(string prefix)
	{
		var eployeesNames = _db.Employees
			.Where(emp => emp.Name.StartsWith(prefix))
			.Select(emp => emp.Name)
			.Distinct();
		return Json(eployeesNames);
	}

	private SelectList GetDepartmentSelectList()
	{
		var departments = _db.Departments.ToList();
		var departmentSelectList = new SelectList(_mapper.Map<IEnumerable<DepartmentNamesDto>>(departments),
			nameof(DepartmentNamesDto.Id),
			nameof(DepartmentNamesDto.Name));
		return departmentSelectList;
	}

	private IEnumerable<LanguageDto> GetLanguagesList()
	{
		var languages = _db.Languages.ToList();
		var languagesList = _mapper.Map<IEnumerable<LanguageDto>>(languages);
		return languagesList;
	}
}
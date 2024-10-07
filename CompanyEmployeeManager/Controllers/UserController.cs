using CompanyEmployeeManager.Data;
using CompanyEmployeeManager.Data.Entities;
using CompanyEmployeeManager.Helpers;
using CompanyEmployeeManager.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployeeManager.Controllers;

[Authorize]
[Route("user")]
public class UserController : Controller
{
	private readonly ApplicationDbContext _db;

	public UserController(ApplicationDbContext db)
	{
		_db = db;
	}

	[HttpGet("add")]
	public IActionResult Add()
	{
		return View();
	}

	[HttpPost("add")]
	public IActionResult Add(RegisterModel model)
	{
		if (ModelState.IsValid)
		{
			var existingUser = _db.Users.SingleOrDefault(u => u.UserName == model.Username);
			if (existingUser != null)
			{
				ModelState.AddModelError("", "User with this name already exists.");
				return View(model);
			}

			var hashedPassword = AuthHelper.HashPassword(model.Password);

			var user = new User
			{
				UserName = model.Username,
				Password = hashedPassword,
				LastActivity = DateTime.UtcNow
			};
			_db.Users.Add(user);
			_db.SaveChanges();

			return Redirect("/");
		}

		return View(model);
	}
}
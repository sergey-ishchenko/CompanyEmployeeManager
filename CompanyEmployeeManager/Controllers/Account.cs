using System.Security.Claims;
using CompanyEmployeeManager.Data;
using CompanyEmployeeManager.Data.Entities;
using CompanyEmployeeManager.Helpers;
using CompanyEmployeeManager.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployeeManager.Controllers;

public class AccountController : Controller
{
	private readonly ApplicationDbContext _db;

	public AccountController(ApplicationDbContext db)
	{
		_db = db;
	}

	public ActionResult Register()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public ActionResult Register(RegisterModel model)
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

			return RedirectToAction("Login");
		}

		return View(model);
	}

	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginModel model)
	{
		if (ModelState.IsValid)
		{
			var hashedPassword = AuthHelper.HashPassword(model.Password);

			var user = _db.Users.SingleOrDefault(u => u.UserName == model.UserName && u.Password == hashedPassword);

			if (user != null)
			{
				var claims = new List<Claim>
				{
					new(ClaimTypes.Name, user.UserName)
				};

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				var authProperties = new AuthenticationProperties
				{
					AllowRefresh = true,
					IsPersistent = true,
					ExpiresUtc = DateTime.UtcNow.AddDays(1)
				};
				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity),
					authProperties
				);

				user.LastActivity = DateTime.UtcNow;
				await _db.SaveChangesAsync();

				return RedirectToAction("Index", "Employee");
			}

			ModelState.AddModelError("", "Incorrect username or password.");
		}

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync();
		return RedirectToAction("Index", "Employee");
	}
}
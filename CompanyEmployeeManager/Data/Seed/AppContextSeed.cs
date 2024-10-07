using CompanyEmployeeManager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using CompanyEmployeeManager.Data.Entities;

namespace CompanyEmployeeManager.Data.Seed
{
	public class AppContextSeed
	{
		public static async Task SeedAsync(
			ApplicationDbContext dbContext,
			IWebHostEnvironment env,
			ILogger logger)
		{
			try
			{
				await ApplyMigrations(dbContext, logger);
				await AddDepartments(dbContext, env);
				await AddLanguages(dbContext, env);
			}
			catch (Exception ex)
			{
				logger.LogError("An error occurred seeding the DB.", ex);
			}
		}

		private static async Task AddLanguages(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			if (!await dbContext.Languages.AnyAsync())
			{
				string data = GetData(env, "Languages.json");
				var items = JsonSerializer.Deserialize<List<Language>>(data);
				foreach (var item in items)
				{
					dbContext.Languages.Add(item);
				}
				await dbContext.SaveChangesAsync();
			}
		}

		private static async Task AddDepartments(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			if (!await dbContext.Departments.AnyAsync())
			{
				string data = GetData(env, "Departments.json");
				var items = JsonSerializer.Deserialize<List<Department>>(data);
				foreach (var item in items)
				{
					dbContext.Departments.Add(item);
				}
				await dbContext.SaveChangesAsync();
			}
		}

		private static async Task ApplyMigrations(ApplicationDbContext dbContext, ILogger logger)
		{
			if (dbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
			{
				// applies any pending migration into our database
				await dbContext.Database.MigrateAsync();
				logger.LogDebug("Migrations were applied.");
			}
		}

		private static string GetData(IWebHostEnvironment env, string file)
		{
			string rootPath = env.ContentRootPath;
			string filePath = Path.GetFullPath(Path.Combine(rootPath, "Data", "Seed", $"{file}"));
			using var r = new StreamReader(filePath);
			string json = r.ReadToEnd();

			return json;
		}
	}
}
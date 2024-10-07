using CompanyEmployeeManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployeeManager.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<Experience> Experiences { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Employee>()
				.HasMany(emp => emp.Experiences)
				.WithOne(exp => exp.Employee)
				.HasForeignKey(exp => exp.EmployeeId);


			builder.Entity<Employee>()
				.HasQueryFilter(emp => !emp.IsDeleted);
		}
	}
}

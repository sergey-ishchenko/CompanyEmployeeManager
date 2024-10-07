using CompanyEmployeeManager;
using CompanyEmployeeManager.Data;
using CompanyEmployeeManager.Data.Seed;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
	logging.AddConsole();
	logging.AddDebug();
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "employee",
	pattern: "{action=Index}/{id?}",
	defaults: new { controller = "Employee" });
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");


//Seeding
using (var scope = app.Services.CreateScope())
{
	var scopedProvider = scope.ServiceProvider;

	var applicationDbContext = scopedProvider.GetRequiredService<ApplicationDbContext>();

	var env = scopedProvider.GetRequiredService<IWebHostEnvironment>();

	await AppContextSeed.SeedAsync(applicationDbContext, env, app.Logger);
}


app.Run();

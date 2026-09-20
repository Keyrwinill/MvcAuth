using Microsoft.AspNetCore.Authentication.Cookies;
using MvcAuth.Data;
using MvcAuth.Data.Repositories;
using MvcAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Connection string for SQL Server database
builder.Services.AddSingleton<SqlConnectionFactory>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<PasswordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services
	.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		// Redirect unauthenticated users here when authorization is required.
		options.LoginPath = "/Account/Login";

		// Redirect authenticated users here when they do not have permission.
		options.AccessDeniedPath = "/Account/AccessDenied";

		// Authentication ticket is valid for 30 minutes.
		options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

		// Renew the authentication ticket when an active user
		// has progressed sufficiently through the expiration window.
		options.SlidingExpiration = true;

		// Prevent JavaScript from directly reading the authentication cookie.
		options.Cookie.HttpOnly = true;

		// Only send the authentication cookie over HTTPS.
		options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

		// Restrict sending the cookie in cross-site request contexts.
		options.Cookie.SameSite = SameSiteMode.Lax;
	});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.Run();

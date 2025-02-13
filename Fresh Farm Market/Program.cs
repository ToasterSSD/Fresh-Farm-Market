using Fresh_Farm_Market.Model;
using Fresh_Farm_Market.MiddleWare;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Fresh_Farm_Market.Migrations;
using Microsoft.AspNetCore.DataProtection;
using Fresh_Farm_Market;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddDataProtection(); // Add Data Protection


builder.Services.AddScoped<DataProtectionService>();


// Updated Identity configuration
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
	options.User.RequireUniqueEmail = true;  // Enable unique email requirement

	// Optional: Add additional password requirements
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 12;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = true;

	// Account lockout settings
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Set lockout time to 2 minutes
	options.Lockout.MaxFailedAccessAttempts = 3; // Set max failed access attempts
	options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IAuditLogger, AuditLogger>();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(5); // Set session timeout
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseStatusCodePagesWithReExecute("/errors/{0}");
	app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Enable session
app.UseMiddleware<SessionTimeoutMiddleware>(); // Register the middleware

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.MapRazorPages();

app.Run();

using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillHubeduardo.Application.Interfaces;
using SkillHubeduardo.Application.Services;
using SkillHubeduardo.Domain.Entities;
using SkillHubeduardo.Domain.Interfaces;
using SkillHubeduardo.Infrastructure.Persistence;
using SkillHubeduardo.Infrastructure.Repositories;
using SkillHubeduardo.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Fixed port for Web application
builder.WebHost.UseUrls("http://localhost:7000");

// Shared database path across Web and Api in the user home directory
var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SkillHubeduardo.db");
var defaultConn = $"Data Source={dbPath}";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains(".db"))
{
    connectionString = defaultConn;
}

builder.Services.AddDbContext<SkillHubeduardoDbContext>(options =>
{
    if (connectionString.Contains(".db") || connectionString.Contains("Data Source=") && !connectionString.Contains(";"))
    {
        options.UseSqlite(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

// Configure ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<SkillHubeduardoDbContext>()
.AddDefaultTokenProviders();

// Explicitly bind default authentication to Cookie scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Workshops";
    options.Cookie.Name = "SkillHubeduardo.Web.Session";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<IWorkshopRepository, WorkshopRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IWorkshopService, WorkshopService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPdfGenerator, QuestPdfCertificateGenerator>();
builder.Services.AddScoped<IExcelService, ClosedXmlExcelService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ensure Database and Seed initial data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SkillHubeduardoDbContext>();
    var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

    context.Database.EnsureCreated();
    await DatabaseSeeder.SeedAsync(context, roleMgr, userMgr);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workshops}/{action=Index}/{id?}");

app.Run();
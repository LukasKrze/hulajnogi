using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Repositories;
using ScooterRentalApp.Data;
using ScooterRentalApp.Models;
using ScooterRentalApp.Models.Validators;
using XLocalizer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Client>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddXLocalizer<LocalPackageSourceInfo>(options =>
{
    options.ModelBindingErrors = new XLocalizer.ErrorMessages.ModelBindingErrors
    {
        AttemptedValueIsInvalidAccessor = "Podana wartoœæ '{0}' nie jest poprawna dla pola {1}.", 
        MissingBindRequiredValueAccessor = "A value for the '{0}' parameter or property was not provided.",
        // ...
    };
}); ;
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IValidator<ScooterViewModel>, ScooterValidator>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapControllers();

var scope = app.Services.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Client>>();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
SeedData.Initialize(dbContext, userManager).Wait();

app.Run();

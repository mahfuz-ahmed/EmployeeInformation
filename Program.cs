using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

builder.Services.AddDbContext<EmployeeInfo.Persistence.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<EmployeeInfo.Persistence.ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<EmployeeInfo.UnitOfWork.IUnitOfWork, EmployeeInfo.UnitOfWork.UnitOfWork>();
builder.Services.AddScoped<EmployeeInfo.Services.Interfaces.IEmployeeInfoService, EmployeeInfo.Services.Implementation.EmployeeInfoService>();
builder.Services.AddScoped<EmployeeInfo.Services.Interfaces.IDesignationService, EmployeeInfo.Services.Implementation.DesignationService>();
builder.Services.AddScoped<EmployeeInfo.Services.Interfaces.ISalaryService, EmployeeInfo.Services.Implementation.SalaryService>();

builder.Services.AddControllersWithViews(options =>
{
    var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

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
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();

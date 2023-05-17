using Crud_Application;
using Crud_Application.Services;
using Crud_Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache(); // Add distributed memory cache service
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Set HttpOnly option for session cookie
    options.Cookie.IsEssential = true; // Set IsEssential option for session cookie
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IEntryService,EntryService>();
builder.Services.AddScoped<IAuditService,AuditService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "AuthCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/User/Login"; // Specify the login page URL
    options.AccessDeniedPath = "/Home/AccessDenied"; // Specify the access denied page URL
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
app.UseStaticFiles();


app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "users",
        pattern: "User/{action}/{id?}",
        defaults: new { controller = "User" });
});

app.Run();

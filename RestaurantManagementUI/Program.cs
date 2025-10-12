using RestaurantManagementUI.Data;
using RestaurantManagementUI.Interfaces;
using RestaurantManagementUI.Repository;
using RestaurantManagementUI.Unit_of_work;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add MVC services
builder.Services.AddControllersWithViews();

// Add memory cache (required for session)
builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<DBConnection>();

builder.Services.AddScoped<ICategory,CategoryRepo>();
builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<ITable, TableRepo>();
builder.Services.AddScoped<IStaff, StaffRepo>();
builder.Services.AddScoped<IPOS, POSRepo>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

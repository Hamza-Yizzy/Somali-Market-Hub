using Microsoft.EntityFrameworkCore;
using Somali_Market_Hub.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SMHDbContext>(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("SMHConnectionString")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=CreateUser}/{id?}")
    .WithStaticAssets();


app.Run();

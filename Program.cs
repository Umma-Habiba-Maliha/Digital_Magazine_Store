using DigitalMagazineStore.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace DigitalMagazineStore
{
public class Program
{
public static async Task Main(string[] args)
{
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString(&quot;DefaultConnection&quot;) ?? throw new
InvalidOperationException(&quot;Connection string &#39;DefaultConnection&#39; not found.&quot;);
builder.Services.AddDbContext&lt;ApplicationDbContext&gt;(options =&gt;
options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services
.AddIdentity&lt;IdentityUser,IdentityRole&gt;(options =&gt; options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores&lt;ApplicationDbContext&gt;()
.AddDefaultUI()
.AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient&lt;IHomeRepository,HomeRepository&gt;();
builder.Services.AddTransient&lt;ICartRepository, CartRepository&gt;();
builder.Services.AddTransient&lt;IUserOrderRepository, UserOrderRepository&gt;();
var app = builder.Build();
//using (var scope=app.Services.CreateScope())
//{
// await DbSeeder.SeedDefaultData(scope.ServiceProvider);
// }
// }
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
app.UseMigrationsEndPoint();
}
else
{
app.UseExceptionHandler(&quot;/Home/Error&quot;);
// The default HSTS value is 30 days. You may want to change this for production scenarios, see
https://aka.ms/aspnetcore-hsts.
app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
name: &quot;default&quot;,
pattern: &quot;{controller=Home}/{action=Index}/{id?}&quot;)
.WithStaticAssets();
app.MapRazorPages()
.WithStaticAssets();
app.Run();
}
}
}
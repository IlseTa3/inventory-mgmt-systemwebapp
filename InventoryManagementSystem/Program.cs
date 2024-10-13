using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure MySQL connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36))));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireStockmanagerRole", policy => policy.RequireRole("Stockmanager"));
    options.AddPolicy("RequireStockemployeeRole", policy => policy.RequireRole("Stockemployee"));
    options.AddPolicy("StockemployeeReadUpdate", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Stockemployee") &&
            (context.Resource.ToString().Contains("Read") || context.Resource.ToString().Contains("Update"))
        ));
    options.AddPolicy("StockmanagerOrAdmin", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Stockmanager") || context.User.IsInRole("Administrator")
        ));
});

builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


var serviceProvider = app.Services.CreateScope().ServiceProvider;

var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//Aanmaak rollen (already done) - mag in commentaar
/*await roleManager.CreateAsync(new IdentityRole("Stockemployee"));
await roleManager.CreateAsync(new IdentityRole("Stockmanager"));
await roleManager.CreateAsync(new IdentityRole("Administrator"));
*/

var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//Toewijzing rollen aan users (already done, mag in commentaar)
/*var adminUser = await userManager.FindByEmailAsync("ilset@retroconstruct.be");
await userManager.AddToRoleAsync(adminUser, "Administrator");

var stockEmloyeeUser = await userManager.FindByEmailAsync("mikec@retroconstruct.be");
await userManager.AddToRoleAsync(stockEmloyeeUser, "Stockemployee");

var stockManagerUser = await userManager.FindByEmailAsync("johannesdb@retroconstruct.be");
await userManager.AddToRoleAsync(stockManagerUser, "Stockmanager");
*/

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
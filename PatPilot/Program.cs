using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatPilot.Data;
using PatPilot.Models;
using PatPilot.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Ajouter DbContext
// Ajouter DbContext pour l'application
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurer Identity avec les paramètres de mot de passe et les rôles
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddRoles<IdentityRole>() // Assurez-vous d'ajouter les rôles
.AddEntityFrameworkStores<ApplicationDbContext>(); // Ajout du store Entity Framework pour Identity

// Ajouter les contrôleurs et vues
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IGateauRepository, GateauRepository>();

builder.Services.AddSession();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSession();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Appeler SeedRolesAndUsersAsync
using (var scope = app.Services.CreateScope())
{
    //var services = scope.ServiceProvider;
    
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    // Essayer d'obtenir le UserManager et le RoleManager après l'enregistrement des services
    //var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed les rôles et utilisateurs
    await context.SeedRolesAndUsersAsync(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

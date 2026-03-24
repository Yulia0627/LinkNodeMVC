using LinkNodeInfrastructure;
using LinkNodeDomain.Model; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbLinkNodeContext>(option => option.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<DbLinkNodeContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        // ВАЖЛИВО: додано <IdentityRole<int>>, щоб збігалося з AddIdentity
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Сталася помилка під час наповнення БД: " + ex.Message);
        // Додай цей рядок, щоб побачити помилку прямо в консолі при запуску
        Console.WriteLine("ПОМИЛКА ІНІЦІАЛІЗАЦІЇ: " + ex.Message);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets(); 

app.UseRouting();


app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
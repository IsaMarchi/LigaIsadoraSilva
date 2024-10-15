using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Helpers;
using LigaIsadoraSilva.Data.Repositories;
using LigaIsadoraSilva.Data.Interface;
using LigaIsadoraSilva.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity configuration
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequiredLength = 6;

    // Desabilitar a confirmação de conta via e-mail
    cfg.SignIn.RequireConfirmedAccount = false;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DataContext>();

//Services
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/NotAuthorized";
    options.AccessDeniedPath = "/Account/NotAuthorized";
});

// Register repositories and services
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        var userHelper = services.GetRequiredService<IUserHelper>();
        var seed = new SeedDb(context, userHelper);  // Passar o userHelper aqui
        await seed.SeedAsync(services);

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization middlewares
app.UseAuthentication();  // Ensure this comes before UseAuthorization
app.UseAuthorization();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

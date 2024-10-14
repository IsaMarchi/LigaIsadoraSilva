using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LigaIsadoraSilva.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;
        private UserHelper _userHelper;

        public SeedDb(DataContext context, UserHelper userHelper)
        {
            _context = context;
            _random = new Random();
            _userHelper = userHelper;
    }

        private void AddClub(string name, string coach, string stadium, string logo, DateTime foundation)
        {
            _context.Clubs.Add(new FootballTeam
            {
                Name = name,
                Coach = coach,
                Stadium = stadium,
                Photo = logo,
                Foundation = foundation,
                Points = _random.Next(100),
                MatchesPlayed = _random.Next(100),
            });
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Staff");

            var user = await _userHelper.GetUserByEmailAsync("marchiisadora@gmail.com");
            if (user == null)
            {

                user = new User
                {
                    Name = "Isadora",
                    Surname = "Silva",
                    Email = "marchiisadora@gmail.com",
                    UserName = "marchiisadora@gmail.com",
                    //EmailConfirmed = true,
                    TwoFactorEnabled = false,
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "admin");
            }

            var IsInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!IsInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            //var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            //if (!await roleManager.RoleExistsAsync("Admin"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Admin"));
            //}

            //if (!await roleManager.RoleExistsAsync("Staff"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Staff"));
            //}

            //if (!await roleManager.RoleExistsAsync("Team"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Team"));
            //}

            //if (!await roleManager.RoleExistsAsync("Anonimous"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Anonimous"));
            //}


        }

    }
}


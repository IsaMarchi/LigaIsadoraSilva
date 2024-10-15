using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using LigaIsadoraSilva.Helpers;
using LigaIsadoraSilva.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;

namespace LigaIsadoraSilva.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ITeamRepository _teamRepository;  // Repositório de times

        public AccountController(IConfiguration configuration,
            IUserHelper userHelper,
            ITeamRepository teamRepository)
        {
            _userHelper = userHelper;
            _teamRepository = teamRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel();
            model.FootballTeams = _teamRepository.GetFootballTeams().ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            model.FootballTeams = _teamRepository.GetFootballTeams().ToList();
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    user = new User
                    {
                        Name = model.FirstName,
                        Surname = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                    };

                    // Associar o usuário ao FootballTeam, se selecionado
                    if (model.FootballTeamId.HasValue)
                    {
                        var footballTeam = await _teamRepository.GetFootballTeamByIdAsync(model.FootballTeamId.Value);
                        if (footballTeam != null)
                        {
                            user.FootballTeam = footballTeam; // Associar o time de futebol ao usuário
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Football Team not found");
                            return View(model);
                        }
                    }

                    // Adicionar usuário
                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created");
                        return View(model);
                    }

                    // Associar o usuário ao papel apropriado (Club ou Staff)
                    await _userHelper.AddUserToRoleAsync(user, model.UserRole);
                    if (!await _userHelper.IsUserInRoleAsync(user, model.UserRole))
                    {
                        ModelState.AddModelError(string.Empty, "Failed to assign role to the user");
                        return View(model);
                    }

                    // Redirecionar após o registro bem-sucedido
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "User with this email already exists");
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirstName = user.Name;
                model.LastName = user.Surname;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    user.Name = model.FirstName;
                    user.Surname = model.LastName;
                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}

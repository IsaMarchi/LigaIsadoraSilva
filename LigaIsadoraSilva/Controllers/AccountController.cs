using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using LigaIsadoraSilva.Helpers;
using LigaIsadoraSilva.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LigaIsadoraSilva.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ITeamRepository _teamRepository;
        private readonly IMailHelper _emailService;  // Serviço de envio de email

        public AccountController(IConfiguration configuration,
                                 IUserHelper userHelper,
                                 ITeamRepository teamRepository,
                                 IMailHelper emailService)
        {
            _userHelper = userHelper;
            _teamRepository = teamRepository;
            _emailService = emailService;
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
                    // Verifique se o usuário está no papel de Admin
                    var user = await _userHelper.GetUserByEmailAsync(model.UserName);
                    if (user != null && await _userHelper.IsUserInRoleAsync(user, "Admin")) // Assumindo que o nome do papel é "Admin"
                    {
                        return RedirectToAction("Index", "Dashboard"); // Redirecione para o Dashboard
                    }

                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home"); // Redirecione para a Home para outros usuários
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
            var model = new RegisterNewUserViewModel
            {
                FootballTeams = _teamRepository.GetFootballTeams().ToList()
            };
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

                    if (model.FootballTeamId.HasValue)
                    {
                        var footballTeam = await _teamRepository.GetFootballTeamByIdAsync(model.FootballTeamId.Value);
                        if (footballTeam != null)
                        {
                            user.FootballTeam = footballTeam;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Football Team not found");
                            return View(model);
                        }
                    }

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, model.UserRole);
                    if (!await _userHelper.IsUserInRoleAsync(user, model.UserRole))
                    {
                        ModelState.AddModelError(string.Empty, "Failed to assign role to the user");
                        return View(model);
                    }

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
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault()?.Description);
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
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault()?.Description);
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

        // Forgot Password
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                var token = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { email = model.Email, token = token }, protocol: HttpContext.Request.Scheme);

                 _emailService.SendEmail(user.Email, "Reset Password",$"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // Reset Password
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Error");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Obtenha o usuário pelo e-mail
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}



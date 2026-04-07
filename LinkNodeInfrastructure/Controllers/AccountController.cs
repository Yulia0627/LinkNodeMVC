using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LinkNodeInfrastructure.ViewModels;
using LinkNodeDomain.Model;

namespace LinkNodeInfrastructure.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    Country = model.Country,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

              
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    return RedirectToAction("ChooseRole", "Account", new { userId = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null) return View(model);

                    var roles = await _userManager.GetRolesAsync(user);

                    
                    var upperRoles = roles.Select(r => r.ToUpper()).ToList();

                    if (upperRoles.Contains("FREELANCER"))
                        return RedirectToAction("Index", "Vacancies");

                    if (upperRoles.Contains("CLIENT"))
                        return RedirectToAction("Index", "Freelancers");

                    if (upperRoles.Contains("ADMIN"))
                        return RedirectToAction("Index", "Admin");

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChooseRole(int userId) 
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetRole(int userId, string roleName) 
        {
            
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound();

            roleName = roleName.ToUpper();

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded) return BadRequest("Не вдалося додати роль.");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return roleName == "FREELANCER"
                ? RedirectToAction("Create", "Freelancers", new { id = user.Id })
                : RedirectToAction("Create", "Clients", new { id = user.Id });
        }

    }
}
using DoaHub.Models;
using DoaHub.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoaHub.Controllers
{
    // Login, cadastro e logout. Fica separado do HomeController para deixar
    // claro o que é "autenticação" e o que é "conteúdo do site".
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            if (modelo.TipoPerfil != Roles.Doador && modelo.TipoPerfil != Roles.Instituicao)
            {
                ModelState.AddModelError(string.Empty, "Tipo de perfil inválido.");
                return View(modelo);
            }

            var usuario = new ApplicationUser
            {
                UserName = modelo.Email,
                Email = modelo.Email,
                NomeCompleto = modelo.NomeCompleto
            };

            var resultado = await _userManager.CreateAsync(usuario, modelo.Senha);

            if (resultado.Succeeded)
            {
                await _userManager.AddToRoleAsync(usuario, modelo.TipoPerfil);
                await _signInManager.SignInAsync(usuario, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var erro in resultado.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);

            return View(modelo);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(modelo);

            var resultado = await _signInManager.PasswordSignInAsync(
                modelo.Email, modelo.Senha, modelo.LembrarMe, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "E-mail ou senha incorretos.");
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

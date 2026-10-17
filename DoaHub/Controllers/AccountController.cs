using DoaHub.Models;
using DoaHub.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoaHub.Controllers
{
    // Login e os dois cadastros (doador / instituição) ficam aqui, separados
    // do HomeController, que só cuida de redirecionar para o lugar certo.
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ---------------- Login ----------------

        [HttpGet("/entrar")]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost("/entrar")]
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

            ModelState.AddModelError(string.Empty, "E-mail ou senha incorretos. Confira os dados ou recupere a senha.");
            return View(modelo);
        }

        // ---------------- Cadastro de doador ----------------

        [HttpGet("/cadastro/doador")]
        public IActionResult RegistrarDoador()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterDoadorViewModel());
        }

        [HttpPost("/cadastro/doador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarDoador(RegisterDoadorViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var usuario = new ApplicationUser
            {
                UserName = modelo.Email,
                Email = modelo.Email,
                NomeCompleto = modelo.NomeCompleto,
                Telefone = modelo.Telefone,
                TipoSanguineo = modelo.TipoSanguineo,
                Cidade = modelo.Cidade,
                Estado = modelo.Estado,
                QuerAvisos = modelo.QuerAvisos
            };

            var resultado = await _userManager.CreateAsync(usuario, modelo.Senha);

            if (resultado.Succeeded)
            {
                await _userManager.AddToRoleAsync(usuario, Roles.Doador);
                await _signInManager.SignInAsync(usuario, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var erro in resultado.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);

            return View(modelo);
        }

        // ---------------- Cadastro de instituição ----------------

        [HttpGet("/cadastro/instituicao")]
        public IActionResult RegistrarInstituicao()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterInstituicaoViewModel());
        }

        [HttpPost("/cadastro/instituicao")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarInstituicao(RegisterInstituicaoViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var usuario = new ApplicationUser
            {
                UserName = modelo.EmailContato,
                Email = modelo.EmailContato,
                NomeCompleto = modelo.NomeInstituicao,
                Cnpj = modelo.Cnpj,
                TipoInstituicao = modelo.Tipo,
                Endereco = modelo.Endereco,
                Telefone = modelo.TelefoneContato,
                Responsavel = modelo.Responsavel,
                Cargo = modelo.Cargo
            };

            var resultado = await _userManager.CreateAsync(usuario, modelo.SenhaAcesso);

            if (resultado.Succeeded)
            {
                await _userManager.AddToRoleAsync(usuario, Roles.Instituicao);
                await _signInManager.SignInAsync(usuario, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var erro in resultado.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);

            return View(modelo);
        }

        // ---------------- Logout / acesso negado ----------------

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

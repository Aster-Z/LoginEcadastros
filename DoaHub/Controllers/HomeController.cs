using DoaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoaHub.Controllers
{
    public class HomeController : Controller
    {
        // Quem já está logado é mandado direto para o painel do seu perfil.
        // Quem não está logado vê a home pública, com os botões de Entrar/Criar conta.
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole(Roles.Doador))
                    return RedirectToAction("Index", "Doador");

                if (User.IsInRole(Roles.Instituicao))
                    return RedirectToAction("Index", "Instituicao");
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

using DoaHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoaHub.Controllers
{
    // Só quem tem o perfil "Doador" entra aqui. Quem tentar acessar sem estar logado
    // é mandado para a tela de login; quem está logado com outro perfil cai em AccessDenied.
    [Authorize(Roles = Roles.Doador)]
    public class DoadorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

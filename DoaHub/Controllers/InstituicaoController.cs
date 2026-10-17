using DoaHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoaHub.Controllers
{
    // Só quem tem o perfil "InstituicaoColetora" entra aqui.
    [Authorize(Roles = Roles.Instituicao)]
    public class InstituicaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

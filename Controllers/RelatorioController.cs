using Microsoft.AspNetCore.Mvc;

namespace esii.Controllers
{
    public class RelatorioController : Controller
    {
        public IActionResult Relatorio()
        {
            return View(); // carrega Views/Relatorio/Relatorio.cshtml
        }
    }
}

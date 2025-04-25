using Microsoft.AspNetCore.Mvc;
using esii.Models;
using esii.Services;

namespace esii.Controllers
{
    public class JurosCompostosController : Controller
    {
        [HttpGet]
        public IActionResult Simular()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Simular(JurosCompostosViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            decimal montante = JurosCompostosService.Calcular(model.CapitalInicial, model.TaxaJuros, model.Periodos);
            ViewBag.MontanteFinal = montante.ToString("F2");
            return View(model);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using esii.Models;
using esii.Services;

namespace esii.Controllers
{
    public class LucroController : Controller
    {
        [HttpGet]
        public IActionResult Simular()
        {
            return View(new LucroViewModel());
        }


        [HttpPost]
        public IActionResult Simular(LucroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var lucroBruto = model.MontanteFinal - model.CapitalInicial;
            var imposto = lucroBruto * (model.TaxaImposto / 100);
            var lucroLiquido = lucroBruto - imposto;

            model.LucroBruto = lucroBruto;
            model.ImpostoValor = imposto;
            model.LucroLiquido = lucroLiquido;

            return View(model);
        }
    }
}

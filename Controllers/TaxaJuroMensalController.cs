using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using esii.Context;
using esii.Entities;
using esii.Models;

namespace esii.Controllers
{
    [Authorize]
    [Route("Parametros/TaxasJuro")]
    public class TaxaJuroMensalController : Controller
    {
        private readonly MyDbContext _context;

        public TaxaJuroMensalController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var taxas = await _context.TaxaJuroMensal.ToListAsync();
            return View(taxas);
        }

        [HttpGet("Create")]
        public IActionResult Create() => View();

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaxaJuroMensalViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var novaTaxa = new TaxaJuroMensal
            {
                Mes = model.Mes,
                Ano = model.Ano,
                Taxa = model.Taxa,
                Descricao = model.Descricao
            };

            _context.TaxaJuroMensal.Add(novaTaxa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var taxa = await _context.TaxaJuroMensal.FindAsync(id);
            if (taxa == null) return NotFound();

            var vm = new TaxaJuroMensalViewModel
            {
                Id = taxa.Id,
                Mes = taxa.Mes,
                Ano = taxa.Ano,
                Taxa = taxa.Taxa,
                Descricao = taxa.Descricao
            };

            return View(vm);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaxaJuroMensalViewModel model)
        {
            if (id != model.Id || !ModelState.IsValid) return View(model);

            var taxa = await _context.TaxaJuroMensal.FindAsync(id);
            if (taxa == null) return NotFound();

            taxa.Mes = model.Mes;
            taxa.Ano = model.Ano;
            taxa.Taxa = model.Taxa;
            taxa.Descricao = model.Descricao;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var taxa = await _context.TaxaJuroMensal.FindAsync(id);
            if (taxa == null) return NotFound();
            return View(taxa);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxa = await _context.TaxaJuroMensal.FindAsync(id);
            if (taxa != null)
            {
                _context.TaxaJuroMensal.Remove(taxa);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
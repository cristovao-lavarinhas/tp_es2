using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using esii.Context;
using esii.Entities;
using esii.Models;

namespace esii.Controllers
{
    [Authorize]
    [Route("Investimentos/Fundos")]
    public class FundoInvestimentoCustomController : Controller
    {
        private readonly MyDbContext _context;

        public FundoInvestimentoCustomController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var dados = await _context.Fundoinvestimentos
                .Include(f => f.Ativo)
                .Where(f => f.Ativo.UtilizadorId == utilizadorId)
                .ToListAsync();

            var viewModels = dados.Select(ToViewModel).ToList();
            return View(viewModels);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var fundo = await _context.Fundoinvestimentos
                .Include(f => f.Ativo)
                .FirstOrDefaultAsync(f => f.AtivoId == id);

            if (fundo == null || !UtilizadorTemPermissao(fundo.Ativo)) return NotFound();

            return View(ToViewModel(fundo));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FundoInvestimentoCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var ativo = new Ativofinanceiro
            {
                UtilizadorId = utilizadorId.Value,
                DataIni = model.DataIni,
                Duracao = model.Duracao,
                Imposto = model.Imposto
            };

            _context.Ativofinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            var fundo = new Fundoinvestimento
            {
                AtivoId = ativo.Id,
                Nome = model.Nome,
                MontanteInvestido = model.MontanteInvestido,
                TaxaJuros = model.TaxaJuros
            };

            _context.Fundoinvestimentos.Add(fundo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var fundo = await _context.Fundoinvestimentos
                .Include(f => f.Ativo)
                .FirstOrDefaultAsync(f => f.AtivoId == id);

            if (fundo == null || !UtilizadorTemPermissao(fundo.Ativo)) return NotFound();

            return View(ToViewModel(fundo));
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FundoInvestimentoCreateViewModel model)
        {
            if (id != model.AtivoId) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var ativo = await _context.Ativofinanceiros.FindAsync(model.AtivoId);
            if (ativo == null || ativo.UtilizadorId != utilizadorId) return NotFound();

            ativo.DataIni = model.DataIni;
            ativo.Duracao = model.Duracao;
            ativo.Imposto = model.Imposto;

            var fundo = await _context.Fundoinvestimentos.FindAsync(model.AtivoId);
            if (fundo == null) return NotFound();

            fundo.Nome = model.Nome;
            fundo.MontanteInvestido = model.MontanteInvestido;
            fundo.TaxaJuros = model.TaxaJuros;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var fundo = await _context.Fundoinvestimentos
                .Include(f => f.Ativo)
                .FirstOrDefaultAsync(f => f.AtivoId == id);

            if (fundo == null || !UtilizadorTemPermissao(fundo.Ativo)) return NotFound();

            return View(ToViewModel(fundo));
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var fundo = await _context.Fundoinvestimentos.FindAsync(id);
            var ativo = await _context.Ativofinanceiros.FindAsync(id);

            if (ativo == null || ativo.UtilizadorId != utilizadorId) return NotFound();

            if (fundo != null) _context.Fundoinvestimentos.Remove(fundo);
            if (ativo != null) _context.Ativofinanceiros.Remove(ativo);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private FundoInvestimentoCreateViewModel ToViewModel(Fundoinvestimento fundo)
        {
            return new FundoInvestimentoCreateViewModel
            {
                AtivoId = fundo.AtivoId,
                DataIni = fundo.Ativo.DataIni,
                Duracao = fundo.Ativo.Duracao,
                Imposto = fundo.Ativo.Imposto,
                Nome = fundo.Nome,
                MontanteInvestido = fundo.MontanteInvestido,
                TaxaJuros = fundo.TaxaJuros
            };
        }

        private int? GetUtilizadorId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : (int?)null;
        }

        private bool UtilizadorTemPermissao(Ativofinanceiro ativo)
        {
            var utilizadorId = GetUtilizadorId();
            return ativo != null && utilizadorId != null && ativo.UtilizadorId == utilizadorId.Value;
        }
    }
}

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using esii.Context;
using esii.Entities;
using esii.Models;
using esii.Commands;

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

            var command = new CreateFundoInvestimentoCommand(_context, model, utilizadorId.Value);
            command.Execute();

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

            var command = new EditFundoInvestimentoCommand(_context, model, utilizadorId.Value);
            command.Execute();

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

            var command = new DeleteFundoInvestimentoCommand(_context, id, utilizadorId.Value);
            command.Execute();

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

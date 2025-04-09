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
    [Route("Investimentos/Depositos")]
    public class DepositoPrazoCustomController : Controller
    {
        private readonly MyDbContext _context;

        public DepositoPrazoCustomController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var dados = await _context.Depositoprazos
                .Include(d => d.Ativo)
                .Where(d => d.Ativo.UtilizadorId == utilizadorId)
                .ToListAsync();

            var viewModels = dados.Select(ToViewModel).ToList();
            return View(viewModels);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var deposito = await _context.Depositoprazos
                .Include(d => d.Ativo)
                .FirstOrDefaultAsync(d => d.AtivoId == id);

            if (deposito == null || !UtilizadorTemPermissao(deposito.Ativo)) return NotFound();

            return View(ToViewModel(deposito));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepositoPrazoCreateViewModel model)
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

            var deposito = new Depositoprazo
            {
                AtivoId = ativo.Id,
                Valor = model.Valor,
                Banco = model.Banco,
                NumConta = model.NumConta,
                Titulares = model.Titulares,
                TaxaJurosAnual = model.TaxaJurosAnual
            };

            _context.Depositoprazos.Add(deposito);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var deposito = await _context.Depositoprazos
                .Include(d => d.Ativo)
                .FirstOrDefaultAsync(d => d.AtivoId == id);

            if (deposito == null || !UtilizadorTemPermissao(deposito.Ativo)) return NotFound();

            return View(ToViewModel(deposito));
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepositoPrazoCreateViewModel model)
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

            var deposito = await _context.Depositoprazos.FindAsync(model.AtivoId);
            if (deposito == null) return NotFound();

            deposito.Valor = model.Valor;
            deposito.Banco = model.Banco;
            deposito.NumConta = model.NumConta;
            deposito.Titulares = model.Titulares;
            deposito.TaxaJurosAnual = model.TaxaJurosAnual;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var deposito = await _context.Depositoprazos
                .Include(d => d.Ativo)
                .FirstOrDefaultAsync(d => d.AtivoId == id);

            if (deposito == null || !UtilizadorTemPermissao(deposito.Ativo)) return NotFound();

            return View(ToViewModel(deposito));
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var deposito = await _context.Depositoprazos.FindAsync(id);
            var ativo = await _context.Ativofinanceiros.FindAsync(id);

            if (ativo == null || ativo.UtilizadorId != utilizadorId) return NotFound();

            if (deposito != null) _context.Depositoprazos.Remove(deposito);
            if (ativo != null) _context.Ativofinanceiros.Remove(ativo);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private DepositoPrazoCreateViewModel ToViewModel(Depositoprazo deposito)
        {
            return new DepositoPrazoCreateViewModel
            {
                AtivoId = deposito.AtivoId,
                DataIni = deposito.Ativo.DataIni,
                Duracao = deposito.Ativo.Duracao,
                Imposto = deposito.Ativo.Imposto,
                Valor = deposito.Valor,
                Banco = deposito.Banco,
                NumConta = deposito.NumConta,
                Titulares = deposito.Titulares,
                TaxaJurosAnual = deposito.TaxaJurosAnual
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

using System.Security.Claims;
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
        public IActionResult Create(DepositoPrazoCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var command = new CreateDepositoCommand(_context, model, utilizadorId.Value);
            var executor = new CommandExecutor();
            executor.Execute(command);

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
        public IActionResult Edit(int id, DepositoPrazoCreateViewModel model)
        {
            if (id != model.AtivoId) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var command = new EditDepositoCommand(_context, model, utilizadorId.Value);
            new CommandExecutor().Execute(command);

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
            
            var tipoAcao = await _context.TipoAcoes.FirstOrDefaultAsync(t => t.Nome == "Remoção");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.UtcNow,
                    Ativo = "Depósito",
                    AtivoId = ativo.Id
                });
                await _context.SaveChangesAsync(); 
            }
            
            if (deposito != null) _context.Depositoprazos.Remove(deposito);
            _context.Ativofinanceiros.Remove(ativo);

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

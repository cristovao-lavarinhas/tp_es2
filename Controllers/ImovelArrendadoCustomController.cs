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
    [Route("Investimentos/Imoveis")]
    public class ImovelArrendadoCustomController : Controller
    {
        private readonly MyDbContext _context;

        public ImovelArrendadoCustomController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var dados = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .Where(i => i.Ativo.UtilizadorId == utilizadorId)
                .ToListAsync();

            var viewModels = dados.Select(ToViewModel).ToList();
            return View(viewModels);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var imovel = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .FirstOrDefaultAsync(i => i.AtivoId == id);

            if (imovel == null || !UtilizadorTemPermissao(imovel.Ativo)) return NotFound();

            return View(ToViewModel(imovel));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ImovelArrendadoCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var command = new CreateImovelArrendadoCommand(_context, model, utilizadorId.Value);
            command.Execute();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var imovel = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .FirstOrDefaultAsync(i => i.AtivoId == id);

            if (imovel == null || !UtilizadorTemPermissao(imovel.Ativo)) return NotFound();

            return View(ToViewModel(imovel));
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ImovelArrendadoCreateViewModel model)
        {
            if (id != model.AtivoId) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var command = new EditImovelArrendadoCommand(_context, model, utilizadorId.Value);
            command.Execute();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var imovel = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .FirstOrDefaultAsync(i => i.AtivoId == id);

            if (imovel == null || !UtilizadorTemPermissao(imovel.Ativo)) return NotFound();

            return View(ToViewModel(imovel));
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var command = new DeleteImovelArrendadoCommand(_context, id, utilizadorId.Value);
            command.Execute();

            return RedirectToAction(nameof(Index));
        }

        private bool AtivoExists(int id)
        {
            return _context.Ativofinanceiros.Any(a => a.Id == id);
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

        private ImovelArrendadoCreateViewModel ToViewModel(Imovelarrendado imovel)
        {
            return new ImovelArrendadoCreateViewModel
            {
                AtivoId = imovel.AtivoId,
                DataIni = imovel.Ativo.DataIni,
                Duracao = imovel.Ativo.Duracao,
                Imposto = imovel.Ativo.Imposto,
                Designacao = imovel.Designacao,
                Localizacao = imovel.Localizacao,
                ValorImovel = imovel.ValorImovel,
                ValorRenda = imovel.ValorRenda,
                ValorMensalCondominio = imovel.ValorMensalCondominio,
                ValorAnualDespesas = imovel.ValorAnualDespesas
            };
        }
    }
}






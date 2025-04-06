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
        public async Task<IActionResult> Create(ImovelArrendadoCreateViewModel model)
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

            var imovel = new Imovelarrendado
            {
                AtivoId = ativo.Id,
                Designacao = model.Designacao,
                Localizacao = model.Localizacao,
                ValorImovel = model.ValorImovel,
                ValorRenda = model.ValorRenda,
                ValorMensalCondominio = model.ValorMensalCondominio,
                ValorAnualDespesas = model.ValorAnualDespesas
            };

            _context.Imovelarrendados.Add(imovel);
            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> Edit(int id, ImovelArrendadoCreateViewModel model)
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

            var imovel = await _context.Imovelarrendados.FindAsync(model.AtivoId);
            if (imovel == null) return NotFound();

            imovel.Designacao = model.Designacao;
            imovel.Localizacao = model.Localizacao;
            imovel.ValorImovel = model.ValorImovel;
            imovel.ValorRenda = model.ValorRenda;
            imovel.ValorMensalCondominio = model.ValorMensalCondominio;
            imovel.ValorAnualDespesas = model.ValorAnualDespesas;

            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var imovel = await _context.Imovelarrendados
                .FirstOrDefaultAsync(i => i.AtivoId == id);
            var ativo = await _context.Ativofinanceiros
                .FirstOrDefaultAsync(a => a.Id == id);

            if (ativo == null || ativo.UtilizadorId != utilizadorId) return NotFound();

            if (imovel != null) _context.Imovelarrendados.Remove(imovel);
            if (ativo != null) _context.Ativofinanceiros.Remove(ativo);

            await _context.SaveChangesAsync();
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






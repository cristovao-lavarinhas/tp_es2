using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using esii.Context;
using esii.Entities;
using esii.Models;

namespace esii.Controllers
{
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
            var dados = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .ToListAsync();

            var viewModels = dados.Select(i => new ImovelArrendadoCreateViewModel
            {
                AtivoId = i.AtivoId,
                UtilizadorId = i.Ativo.UtilizadorId,
                DataIni = i.Ativo.DataIni,
                Duracao = i.Ativo.Duracao,
                Imposto = i.Ativo.Imposto,
                Designacao = i.Designacao,
                Localizacao = i.Localizacao,
                ValorImovel = i.ValorImovel,
                ValorRenda = i.ValorRenda,
                ValorMensalCondominio = i.ValorMensalCondominio,
                ValorAnualDespesas = i.ValorAnualDespesas
            }).ToList();

            return View(viewModels);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var imovel = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .FirstOrDefaultAsync(i => i.AtivoId == id);

            if (imovel == null) return NotFound();

            var viewModel = new ImovelArrendadoCreateViewModel
            {
                AtivoId = imovel.AtivoId,
                UtilizadorId = imovel.Ativo.UtilizadorId,
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

            return View(viewModel);
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
            if (ModelState.IsValid)
            {
                var ativo = new Ativofinanceiro
                {
                    UtilizadorId = model.UtilizadorId,
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

            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var imovel = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .FirstOrDefaultAsync(i => i.AtivoId == id);

            if (imovel == null) return NotFound();

            var viewModel = new ImovelArrendadoCreateViewModel
            {
                AtivoId = imovel.AtivoId,
                UtilizadorId = imovel.Ativo.UtilizadorId,
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

            return View(viewModel);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImovelArrendadoCreateViewModel model)
        {
            if (id != model.AtivoId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var ativo = await _context.Ativofinanceiros.FindAsync(model.AtivoId);
                    if (ativo == null) return NotFound();

                    ativo.UtilizadorId = model.UtilizadorId;
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtivoExists(model.AtivoId)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var imovel = await _context.Imovelarrendados
                .Include(i => i.Ativo)
                .FirstOrDefaultAsync(i => i.AtivoId == id);

            if (imovel == null) return NotFound();

            var viewModel = new ImovelArrendadoCreateViewModel
            {
                AtivoId = imovel.AtivoId,
                UtilizadorId = imovel.Ativo.UtilizadorId,
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

            return View(viewModel);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imovel = await _context.Imovelarrendados
                .FirstOrDefaultAsync(i => i.AtivoId == id);
            var ativo = await _context.Ativofinanceiros
                .FirstOrDefaultAsync(a => a.Id == id);

            if (imovel != null) _context.Imovelarrendados.Remove(imovel);
            if (ativo != null) _context.Ativofinanceiros.Remove(ativo);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtivoExists(int id)
        {
            return _context.Ativofinanceiros.Any(a => a.Id == id);
        }
    }
}





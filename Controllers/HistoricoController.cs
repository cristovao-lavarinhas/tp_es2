using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using esii.Context;
using esii.Models;

namespace esii.Controllers
{
    [Authorize]
    [Route("Historico")]
    public class HistoricoController : Controller
    {
        private readonly MyDbContext _context;

        public HistoricoController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string sort = "DataHora", string order = "desc")
        {
            var utilizadorId = GetUtilizadorId();
            if (utilizadorId == null) return Unauthorized();

            var historicoQuery = _context.HistoricoAcoes
                .Include(h => h.AtivoFinanceiro)
                .Include(h => h.TipoAcao)
                .Where(h => h.AtivoFinanceiro.UtilizadorId == utilizadorId.Value);

            historicoQuery = (sort, order) switch
            {
                ("TipoAcao", "asc") => historicoQuery.OrderBy(h => h.TipoAcao.Nome),
                ("TipoAcao", "desc") => historicoQuery.OrderByDescending(h => h.TipoAcao.Nome),
                ("Ativo", "asc") => historicoQuery.OrderBy(h => h.Ativo),
                ("Ativo", "desc") => historicoQuery.OrderByDescending(h => h.Ativo),
                ("DataHora", "asc") => historicoQuery.OrderBy(h => h.DataAcao),
                _ => historicoQuery.OrderByDescending(h => h.DataAcao)
            };

            var historico = await historicoQuery
                .Select(h => new HistoricoAcaoViewModel
                {
                    TipoAcao = h.TipoAcao.Nome,
                    DataHora = h.DataAcao,
                    AtivoId = h.AtivoId,
                    Ativo = h.Ativo
                })
                .ToListAsync();

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentOrder = order;

            return View("Index", historico);
        }


        private int? GetUtilizadorId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : (int?)null;
        }
    }
}


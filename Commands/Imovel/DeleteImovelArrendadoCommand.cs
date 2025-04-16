using esii.Context;
using esii.Models;
using esii.Entities;
using System;

namespace esii.Commands
{
    public class DeleteImovelArrendadoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly int _ativoId;
        private readonly int _utilizadorId;

        public DeleteImovelArrendadoCommand(MyDbContext context, int ativoId, int utilizadorId)
        {
            _context = context;
            _ativoId = ativoId;
            _utilizadorId = utilizadorId;
        }

        public void Execute()
        {
            var ativo = _context.Ativofinanceiros.FirstOrDefault(a => a.Id == _ativoId && a.UtilizadorId == _utilizadorId);
            if (ativo == null)
                throw new InvalidOperationException("Ativo não encontrado ou sem permissão.");

            var imovel = _context.Imovelarrendados.FirstOrDefault(i => i.AtivoId == _ativoId);
            if (imovel != null)
                _context.Imovelarrendados.Remove(imovel);

            _context.Ativofinanceiros.Remove(ativo);

            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Remoção");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.UtcNow,
                    Ativo = "Imóvel Arrendado",
                    AtivoId = _ativoId
                });
            }

            _context.SaveChanges();
        }
    }
}
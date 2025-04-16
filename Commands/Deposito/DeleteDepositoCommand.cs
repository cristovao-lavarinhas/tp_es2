using esii.Context;
using esii.Models;
using esii.Entities;
using System;

namespace esii.Commands
{
    public class DeleteDepositoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly int _ativoId;
        private readonly int _utilizadorId;

        public DeleteDepositoCommand(MyDbContext context, int ativoId, int utilizadorId)
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

            var deposito = _context.Depositoprazos.FirstOrDefault(d => d.AtivoId == _ativoId);

            if (deposito != null)
                _context.Depositoprazos.Remove(deposito);

            _context.Ativofinanceiros.Remove(ativo);

            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Remoção");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.UtcNow,
                    Ativo = "Depósito a Prazo",
                    AtivoId = _ativoId
                });
            }

            _context.SaveChanges();
        }
    }
}
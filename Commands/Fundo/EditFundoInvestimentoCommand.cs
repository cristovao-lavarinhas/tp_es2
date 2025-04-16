using esii.Context;
using esii.Models;
using esii.Entities;
using System;
using System.Linq;

namespace esii.Commands
{
    public class EditFundoInvestimentoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly FundoInvestimentoCreateViewModel _model;
        private readonly int _utilizadorId;

        public EditFundoInvestimentoCommand(MyDbContext context, FundoInvestimentoCreateViewModel model, int utilizadorId)
        {
            _context = context;
            _model = model;
            _utilizadorId = utilizadorId;
        }

        public void Execute()
        {
            var ativo = _context.Ativofinanceiros.FirstOrDefault(a => a.Id == _model.AtivoId && a.UtilizadorId == _utilizadorId);
            if (ativo == null)
                throw new InvalidOperationException("Ativo não encontrado ou sem permissão.");

            ativo.DataIni = _model.DataIni;
            ativo.Duracao = _model.Duracao;
            ativo.Imposto = _model.Imposto;

            var fundo = _context.Fundoinvestimentos.FirstOrDefault(f => f.AtivoId == _model.AtivoId);
            if (fundo == null)
                throw new InvalidOperationException("Fundo de investimento não encontrado.");

            fundo.Nome = _model.Nome;
            fundo.MontanteInvestido = _model.MontanteInvestido;
            fundo.TaxaJuros = _model.TaxaJuros;

            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Edição");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.UtcNow,
                    Ativo = "Fundo de Investimento",
                    AtivoId = ativo.Id
                });
            }

            _context.SaveChanges();
        }
    }
}    
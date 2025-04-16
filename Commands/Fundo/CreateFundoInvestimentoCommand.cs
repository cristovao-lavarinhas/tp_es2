using esii.Context;
using esii.Models;
using esii.Entities;
using System;
using System.Linq;

namespace esii.Commands
{
    public class CreateFundoInvestimentoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly FundoInvestimentoCreateViewModel _model;
        private readonly int _utilizadorId;

        public CreateFundoInvestimentoCommand(MyDbContext context, FundoInvestimentoCreateViewModel model,
            int utilizadorId)
        {
            _context = context;
            _model = model;
            _utilizadorId = utilizadorId;
        }

        public void Execute()
        {
            var ativo = new Ativofinanceiro
            {
                UtilizadorId = _utilizadorId,
                DataIni = _model.DataIni,
                Duracao = _model.Duracao,
                Imposto = _model.Imposto
            };

            _context.Ativofinanceiros.Add(ativo);
            _context.SaveChanges();

            var fundo = new Fundoinvestimento
            {
                AtivoId = ativo.Id,
                Nome = _model.Nome,
                MontanteInvestido = _model.MontanteInvestido,
                TaxaJuros = _model.TaxaJuros
            };

            _context.Fundoinvestimentos.Add(fundo);
            _context.SaveChanges();

            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Criação");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.UtcNow,
                    Ativo = "Fundo de Investimento",
                    AtivoId = ativo.Id
                });
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Tipo de ação 'Criação' não encontrado.");
            }
        }
    }
}   
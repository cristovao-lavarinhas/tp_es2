using esii.Context;
using esii.Models;
using esii.Entities;
using System;

namespace esii.Commands
{
    public class CreateDepositoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly DepositoPrazoCreateViewModel _model;
        private readonly int _utilizadorId;

        public CreateDepositoCommand(MyDbContext context, DepositoPrazoCreateViewModel model, int utilizadorId)
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
            
            var deposito = new Depositoprazo
            {
                AtivoId = ativo.Id,
                Valor = _model.Valor,
                Banco = _model.Banco,
                NumConta = _model.NumConta,
                Titulares = _model.Titulares,
                TaxaJurosAnual = _model.TaxaJurosAnual
            };

            _context.Depositoprazos.Add(deposito);
            _context.SaveChanges();
            
            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Criação");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.Now.ToUniversalTime(),
                    Ativo = "Depósito a Prazo",
                    AtivoId = ativo.Id
                });

                _context.SaveChanges();


                _context.SaveChanges();
            }

            else
            {
                throw new InvalidOperationException("Tipo de ação 'Criação' não encontrado.");
            }
        }
    }
}

using esii.Context;
using esii.Models;
using esii.Entities;
using System;

namespace esii.Commands
{
    public class EditDepositoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly DepositoPrazoCreateViewModel _model;
        private readonly int _utilizadorId;

        public EditDepositoCommand(MyDbContext context, DepositoPrazoCreateViewModel model, int utilizadorId)
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

            var deposito = _context.Depositoprazos.FirstOrDefault(d => d.AtivoId == _model.AtivoId);
            if (deposito == null)
                throw new InvalidOperationException("Depósito não encontrado.");

            deposito.Valor = _model.Valor;
            deposito.Banco = _model.Banco;
            deposito.NumConta = _model.NumConta;
            deposito.Titulares = _model.Titulares;
            deposito.TaxaJurosAnual = _model.TaxaJurosAnual;

            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Edição");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.UtcNow,
                    Ativo = "Depósito a Prazo",
                    AtivoId = ativo.Id
                });
            }

            _context.SaveChanges();
        }
    }
}
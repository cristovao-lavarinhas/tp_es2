using esii.Context;
using esii.Models;
using esii.Entities;
using System;

namespace esii.Commands
{
    public class EditImovelArrendadoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly ImovelArrendadoCreateViewModel _model;
        private readonly int _utilizadorId;

        public EditImovelArrendadoCommand(MyDbContext context, ImovelArrendadoCreateViewModel model, int utilizadorId)
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

            var imovel = _context.Imovelarrendados.FirstOrDefault(i => i.AtivoId == _model.AtivoId);
            if (imovel == null)
                throw new InvalidOperationException("Imóvel arrendado não encontrado.");

            imovel.Designacao = _model.Designacao;
            imovel.Localizacao = _model.Localizacao;
            imovel.ValorImovel = _model.ValorImovel;
            imovel.ValorRenda = _model.ValorRenda;
            imovel.ValorMensalCondominio = _model.ValorMensalCondominio;
            imovel.ValorAnualDespesas = _model.ValorAnualDespesas;

            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Edição");
            if (tipoAcao != null)
            {
                _context.HistoricoAcoes.Add(new HistoricoAcao
                {
                    TipoAcaoId = tipoAcao.Id,
                    DataAcao = DateTime.UtcNow,
                    Ativo = "Imóvel Arrendado",
                    AtivoId = ativo.Id
                });
            }

            _context.SaveChanges();
        }
    }
}

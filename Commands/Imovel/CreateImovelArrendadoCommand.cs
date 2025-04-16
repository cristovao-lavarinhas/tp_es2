using esii.Context;
using esii.Models;
using esii.Entities;
using System;

namespace esii.Commands
{
    public class CreateImovelArrendadoCommand : ICommand
    {
        private readonly MyDbContext _context;
        private readonly ImovelArrendadoCreateViewModel _model;
        private readonly int _utilizadorId;

        public CreateImovelArrendadoCommand(MyDbContext context, ImovelArrendadoCreateViewModel model, int utilizadorId)
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

            var imovel = new Imovelarrendado
            {
                AtivoId = ativo.Id,
                Designacao = _model.Designacao,
                Localizacao = _model.Localizacao,
                ValorImovel = _model.ValorImovel,
                ValorRenda = _model.ValorRenda,
                ValorMensalCondominio = _model.ValorMensalCondominio,
                ValorAnualDespesas = _model.ValorAnualDespesas
            };

            _context.Imovelarrendados.Add(imovel);

            var tipoAcao = _context.TipoAcoes.FirstOrDefault(t => t.Nome == "Criação");
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
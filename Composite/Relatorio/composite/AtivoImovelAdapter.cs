using esii.Composite.Models;

namespace esii.Composite.Relatorio.composite;

public class AtivoImovelAdapter : IAtivoFinanceiroComposite
{
    private readonly AtivoFinanceiro _base;
    private readonly ImovelArrendado _imovel;

    public AtivoImovelAdapter(AtivoFinanceiro ativo, ImovelArrendado imovel)
    {
        _base = ativo;
        _imovel = imovel;
    }

    public string Nome => _imovel.Designacao;
    public DateTime DataInicio => _base.DataIni;
    public int DuracaoMeses => _base.Duracao;
    public decimal PercentagemImposto => _base.Imposto;

    public decimal CalcularLucro(DateTime inicio, DateTime fim)
    {
        var dataFim = DataInicio.AddMonths(DuracaoMeses);
        if (fim < DataInicio || inicio > dataFim) return 0;

        int meses = Math.Max(0,
            (int)((new[] { dataFim, fim }.Min() - new[] { DataInicio, inicio }.Max()).TotalDays / 30)
        );
        var bruto = _imovel.ValorRenda * meses;
        var cond = _imovel.ValorMensalCondominio * meses;
        var despesas = (_imovel.ValorAnualDespesas / 12) * meses;

        return bruto - cond - despesas;
    }
}

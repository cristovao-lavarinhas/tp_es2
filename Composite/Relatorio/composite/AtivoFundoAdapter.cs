using esii.Composite.Models;

namespace esii.Composite.Relatorio.composite;

public class AtivoFundoAdapter : IAtivoFinanceiroComposite
{
    private readonly AtivoFinanceiro _base;
    private readonly FundoInvestimento _fundo;

    public AtivoFundoAdapter(AtivoFinanceiro ativo, FundoInvestimento fundo)
    {
        _base = ativo;
        _fundo = fundo;
    }

    public string Nome => _fundo.Nome;
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
        return _fundo.MontanteInvestido * (_fundo.TaxaJuros / 100 / 12) * meses;
    }
}

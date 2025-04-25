using esii.Composite.Models;

namespace esii.Composite.Relatorio.composite;

public class AtivoDepositoAdapter : IAtivoFinanceiroComposite
{
    private readonly AtivoFinanceiro _base;
    private readonly DepositoPrazo _dp;

    public AtivoDepositoAdapter(AtivoFinanceiro ativo, DepositoPrazo dp)
    {
        _base = ativo;
        _dp = dp;
    }

    public string Nome => $"Depósito - {_dp.Banco}";
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
        decimal valor = _dp.Valor;
        decimal total = 0;

        for (int i = 0; i < meses; i++)
        {
            var juro = valor * (_dp.TaxaJurosAnual / 100 / 12);
            total += juro;
            valor += juro;
        }

        return total;
    }
}

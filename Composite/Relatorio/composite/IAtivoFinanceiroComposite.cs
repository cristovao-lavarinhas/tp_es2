namespace esii.Composite.Relatorio.composite;

public interface IAtivoFinanceiroComposite
{
    string Nome { get; }
    DateTime DataInicio { get; }
    int DuracaoMeses { get; }
    decimal PercentagemImposto { get; }
    decimal CalcularLucro(DateTime inicio, DateTime fim);
}

namespace esii.Composite.Models;

public class AtivoFinanceiro
{
    public int Id { get; set; }
    public int UtilizadorId { get; set; }
    public DateTime DataIni { get; set; }
    public int Duracao { get; set; }
    public decimal Imposto { get; set; }

    public DepositoPrazo? DepositoPrazo { get; set; }
    public FundoInvestimento? FundoInvestimento { get; set; }
    public ImovelArrendado? ImovelArrendado { get; set; }
}

namespace esii.Composite.Models;

public class FundoInvestimento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal MontanteInvestido { get; set; }
    public decimal TaxaJuros { get; set; }

    public AtivoFinanceiro AtivoFinanceiro { get; set; } = null!;
}

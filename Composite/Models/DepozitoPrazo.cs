namespace esii.Composite.Models;

public class DepositoPrazo
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public string Banco { get; set; } = string.Empty;
    public string NumConta { get; set; } = string.Empty;
    public string Titulares { get; set; } = string.Empty;
    public decimal TaxaJurosAnual { get; set; }

    public AtivoFinanceiro AtivoFinanceiro { get; set; } = null!;
}

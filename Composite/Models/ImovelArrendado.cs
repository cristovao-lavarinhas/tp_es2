namespace esii.Composite.Models;

public class ImovelArrendado
{
    public int Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public decimal ValorImovel { get; set; }
    public decimal ValorRenda { get; set; }
    public decimal ValorMensalCondominio { get; set; }
    public decimal ValorAnualDespesas { get; set; }

    public AtivoFinanceiro AtivoFinanceiro { get; set; } = null!;
}

namespace esii.Models;

public class LucroViewModel
{
    public decimal CapitalInicial { get; set; }
    public decimal MontanteFinal { get; set; }
    public decimal TaxaImposto { get; set; }

    public decimal LucroBruto { get; set; }
    public decimal ImpostoValor { get; set; }
    public decimal LucroLiquido { get; set; }
}

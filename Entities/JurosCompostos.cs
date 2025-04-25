namespace esii.Entities;

public class JurosCompostos
{
    public int Id { get; set; }
    public decimal CapitalInicial { get; set; }
    public decimal TaxaJuros { get; set; }
    public int Periodos { get; set; }
    public decimal MontanteFinal { get; set; }
    public DateTime DataCalculo { get; set; }
}

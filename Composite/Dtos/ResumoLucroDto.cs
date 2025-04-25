namespace esii.Composite.Dtos;

public class ResumoLucroDto
{
    public decimal LucroBruto { get; set; }
    public decimal LucroLiquido { get; set; }
    public decimal MensalBruto { get; set; }
    public decimal MensalLiquido { get; set; }
    public List<AtivoDto> Ativos { get; set; } = new();
}

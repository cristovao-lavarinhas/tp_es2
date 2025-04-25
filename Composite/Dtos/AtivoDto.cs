namespace esii.Composite.Dtos;

public class AtivoDto
{
    public string Tipo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public int DuracaoMeses { get; set; }
}

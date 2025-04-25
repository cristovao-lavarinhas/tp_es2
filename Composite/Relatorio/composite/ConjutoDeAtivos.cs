using esii.Composite.Dtos;

namespace esii.Composite.Relatorio.composite;

public class ConjuntoDeAtivos
{
    private readonly List<IAtivoFinanceiroComposite> _ativos = new();

    public void Adicionar(IAtivoFinanceiroComposite ativo) => _ativos.Add(ativo);

    public decimal CalcularLucro(DateTime inicio, DateTime fim)
        => _ativos.Sum(a => a.CalcularLucro(inicio, fim));

    public decimal CalcularLucroComImposto(DateTime inicio, DateTime fim)
        => _ativos.Sum(a => {
            var lucro = a.CalcularLucro(inicio, fim);
            return lucro > 0 ? lucro * (1 - a.PercentagemImposto / 100) : lucro;
        });

    public List<AtivoDto> ObterAtivosAtivosEntre(DateTime inicio, DateTime fim)
        => _ativos.Where(a => fim >= a.DataInicio && inicio <= a.DataInicio.AddMonths(a.DuracaoMeses))
            .Select(a => new AtivoDto {
                Nome = a.Nome,
                Tipo = a.GetType().Name.Replace("Ativo", ""),
                DataInicio = a.DataInicio,
                DuracaoMeses = a.DuracaoMeses
            }).ToList();
}

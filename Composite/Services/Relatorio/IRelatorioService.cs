using esii.Composite.Dtos;

namespace esii.Composite.Services.Relatorio;

public interface IRelatorioService
{
    Task<ResumoLucroDto> GerarResumoLucro(DateTime inicio, DateTime fim);
    byte[] GerarPdf(ResumoLucroDto resumo, DateTime inicio, DateTime fim);
}

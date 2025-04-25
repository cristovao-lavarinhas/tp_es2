using esii.Composite.Data;
using esii.Composite.Dtos;
using esii.Composite.Relatorio.composite;

namespace esii.Composite.Services.Relatorio;

using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class RelatorioService : IRelatorioService
{
    private readonly AtivosDbContext _context;

    public RelatorioService(AtivosDbContext context)
    {
        _context = context;
    }

    public async Task<ResumoLucroDto> GerarResumoLucro(DateTime inicio, DateTime fim)
    {
        var ativos = new ConjuntoDeAtivos();

        var depositos = await _context.Depositos.Include(d => d.AtivoFinanceiro).ToListAsync();
        var fundos = await _context.Fundos.Include(f => f.AtivoFinanceiro).ToListAsync();
        var imoveis = await _context.Imoveis.Include(i => i.AtivoFinanceiro).ToListAsync();

        foreach (var d in depositos) ativos.Adicionar(new AtivoDepositoAdapter(d.AtivoFinanceiro, d));
        foreach (var f in fundos) ativos.Adicionar(new AtivoFundoAdapter(f.AtivoFinanceiro, f));
        foreach (var i in imoveis) ativos.Adicionar(new AtivoImovelAdapter(i.AtivoFinanceiro, i));

        var bruto = ativos.CalcularLucro(inicio, fim);
        var liquido = ativos.CalcularLucroComImposto(inicio, fim);
        int meses = Math.Max(1, (int)((fim - inicio).TotalDays / 30));

        return new ResumoLucroDto
        {
            LucroBruto = bruto,
            LucroLiquido = liquido,
            MensalBruto = bruto / meses,
            MensalLiquido = liquido / meses,
            Ativos = ativos.ObterAtivosAtivosEntre(inicio, fim)
        };
    }

    public byte[] GerarPdf(ResumoLucroDto resumo, DateTime inicio, DateTime fim)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Content().Column(col =>
                {
                    col.Item().Text("Relatório de Lucros").FontSize(18).Bold();
                    col.Item().Text($"Período: {inicio:yyyy-MM-dd} a {fim:yyyy-MM-dd}");
                    col.Item().Text($"Lucro Bruto: {resumo.LucroBruto:F2} €");
                    col.Item().Text($"Lucro Líquido: {resumo.LucroLiquido:F2} €");
                    col.Item().Text($"Mensal Bruto: {resumo.MensalBruto:F2} €");
                    col.Item().Text($"Mensal Líquido: {resumo.MensalLiquido:F2} €");
                    col.Item().LineHorizontal(1);

                    if (resumo.Ativos.Any())
                    {
                        col.Item().Text("Ativos incluídos:").FontSize(14).Bold();
                        foreach (var ativo in resumo.Ativos)
                        {
                            col.Item().Text($"- {ativo.Tipo} | {ativo.Nome} | {ativo.DataInicio:yyyy-MM-dd} | {ativo.DuracaoMeses} meses");
                        }
                    }
                });
            });
        });

        return doc.GeneratePdf();
    }
}

namespace esii.Services;

public static class JurosCompostosService
{
    public static decimal Calcular(decimal capitalInicial, decimal taxaPercentual, int periodos)
    {
        decimal taxaDecimal = taxaPercentual / 100;
        return capitalInicial * (decimal)Math.Pow((double)(1 + taxaDecimal), periodos);
    }
}

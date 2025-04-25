namespace esii.Services;

public static class LucroService
{
    public static (decimal lucroBruto, decimal impostoValor, decimal lucroLiquido) 
        CalcularLucro(decimal capitalInicial, decimal montanteFinal, decimal taxaImposto)
    {
        var lucroBruto = montanteFinal - capitalInicial;
        var impostoValor = lucroBruto * (taxaImposto / 100);
        var lucroLiquido = lucroBruto - impostoValor;

        return (lucroBruto, impostoValor, lucroLiquido);
    }
}

using System;
using System.Collections.Generic;

namespace esii.Entities;

public partial class Imovelarrendado
{
    public int Id { get; set; }

    public int AtivoId { get; set; }

    public string Designacao { get; set; } = null!;

    public string Localizacao { get; set; } = null!;

    public decimal ValorImovel { get; set; }

    public decimal ValorRenda { get; set; }

    public decimal? ValorMensalCondominio { get; set; }

    public decimal? ValorAnualDespesas { get; set; }

    public virtual Ativofinanceiro Ativo { get; set; } = null!;
}

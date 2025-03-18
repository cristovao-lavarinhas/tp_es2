using System;
using System.Collections.Generic;

namespace esii.Entities;

public partial class Fundoinvestimento
{
    public int Id { get; set; }

    public int AtivoId { get; set; }

    public string Nome { get; set; } = null!;

    public decimal MontanteInvestido { get; set; }

    public decimal TaxaJuros { get; set; }

    public virtual Ativofinanceiro Ativo { get; set; } = null!;
}

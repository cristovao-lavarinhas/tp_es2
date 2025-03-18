using System;
using System.Collections.Generic;

namespace esii.Entities;

public partial class Depositoprazo
{
    public int Id { get; set; }

    public int AtivoId { get; set; }

    public decimal Valor { get; set; }

    public string Banco { get; set; } = null!;

    public string NumConta { get; set; } = null!;

    public string Titulares { get; set; } = null!;

    public decimal TaxaJurosAnual { get; set; }

    public virtual Ativofinanceiro Ativo { get; set; } = null!;
}

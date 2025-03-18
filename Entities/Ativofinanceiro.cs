using System;
using System.Collections.Generic;

namespace esii.Entities;

public partial class Ativofinanceiro
{
    public int Id { get; set; }

    public int UtilizadorId { get; set; }

    public DateOnly DataIni { get; set; }

    public int? Duracao { get; set; }

    public decimal? Imposto { get; set; }

    public virtual ICollection<Depositoprazo> Depositoprazos { get; set; } = new List<Depositoprazo>();

    public virtual ICollection<Fundoinvestimento> Fundoinvestimentos { get; set; } = new List<Fundoinvestimento>();

    public virtual ICollection<Imovelarrendado> Imovelarrendados { get; set; } = new List<Imovelarrendado>();

    public virtual Utilizador Utilizador { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace esii.Entities;

public partial class Tipoutilizador
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Utilizador> Utilizadors { get; set; } = new List<Utilizador>();
}

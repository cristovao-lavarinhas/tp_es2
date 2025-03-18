using System;
using System.Collections.Generic;

namespace esii.Entities;

public partial class Tiporelatorio
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Relatorio> Relatorios { get; set; } = new List<Relatorio>();
}

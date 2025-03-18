using System;
using System.Collections.Generic;

namespace esii.Entities;

public partial class Relatorio
{
    public int Id { get; set; }

    public int UtilizadorId { get; set; }

    public DateOnly DataIni { get; set; }

    public DateOnly DataFim { get; set; }

    public int TipoId { get; set; }

    public virtual Tiporelatorio Tipo { get; set; } = null!;

    public virtual Utilizador Utilizador { get; set; } = null!;
}

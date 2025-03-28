﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace esii.Entities;

public partial class Utilizador
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal? Imposto { get; set; }

    public string Nif { get; set; } = null!;

    public int TipoId { get; set; }

    public virtual ICollection<Ativofinanceiro> Ativofinanceiros { get; set; } = new List<Ativofinanceiro>();

    public virtual ICollection<Relatorio> Relatorios { get; set; } = new List<Relatorio>();

    public virtual Tipoutilizador Tipo { get; set; } = null!;
}

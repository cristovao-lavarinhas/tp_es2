using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace esii.Entities;

public partial class Utilizador
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    
    public string? Pin { get; set; }
    
    public string? OTPCode { get; set; }
    
    public DateTime? OTPExpiry { get; set; }

    public decimal? Imposto { get; set; }

    [Required(ErrorMessage = "O NIF é obrigatório.")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "O NIF deve conter exatamente 9 dígitos.")]
    public string Nif { get; set; } = null!;

    public int TipoId { get; set; }

    public virtual ICollection<Ativofinanceiro> Ativofinanceiros { get; set; } = new List<Ativofinanceiro>();

    public virtual ICollection<Relatorio> Relatorios { get; set; } = new List<Relatorio>();

    public virtual Tipoutilizador Tipo { get; set; } = null!;
}

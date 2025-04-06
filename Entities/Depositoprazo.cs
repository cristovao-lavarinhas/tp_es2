using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esii.Entities;

public partial class Depositoprazo
{
    [Key]
    [ForeignKey("Ativo")]
    public int AtivoId { get; set; }

    public decimal Valor { get; set; }

    [Required]
    public string Banco { get; set; } = null!;

    [Required]
    public string NumConta { get; set; } = null!;

    [Required]
    public string Titulares { get; set; } = null!;

    public decimal TaxaJurosAnual { get; set; }

    public virtual Ativofinanceiro Ativo { get; set; } = null!;
}
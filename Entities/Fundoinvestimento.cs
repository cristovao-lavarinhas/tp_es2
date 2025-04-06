using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esii.Entities;

public partial class Fundoinvestimento
{
    [Key]
    [ForeignKey("Ativo")]
    public int AtivoId { get; set; }

    [Required]
    public string Nome { get; set; } = null!;

    public decimal MontanteInvestido { get; set; }

    public decimal TaxaJuros { get; set; }

    public virtual Ativofinanceiro Ativo { get; set; } = null!;
}


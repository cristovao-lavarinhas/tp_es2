using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using esii.Entities;

namespace esii.Entities
{
    public partial class Imovelarrendado
    {
        [Key]
        [ForeignKey("Ativo")]
        public int AtivoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Designacao { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Localizacao { get; set; } = null!;

        [Column(TypeName = "numeric(15,2)")]
        public decimal ValorImovel { get; set; }

        [Column(TypeName = "numeric(15,2)")]
        public decimal ValorRenda { get; set; }

        [Column(TypeName = "numeric(10,2)")]
        public decimal ValorMensalCondominio { get; set; }

        [Column(TypeName = "numeric(10,2)")]
        public decimal ValorAnualDespesas { get; set; }

        public virtual Ativofinanceiro Ativo { get; set; } = null!;
    }
}


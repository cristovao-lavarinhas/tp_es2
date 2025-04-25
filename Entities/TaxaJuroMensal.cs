using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esii.Entities
{
    public class TaxaJuroMensal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 12)]
        public int Mes { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Ano { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Taxa { get; set; }

        [MaxLength(200)]
        public string? Descricao { get; set; }
    }
}
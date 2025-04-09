using System.ComponentModel.DataAnnotations;

namespace esii.Models
{
    public class FundoInvestimentoCreateViewModel
    {
        public int AtivoId { get; set; }

        [Required]
        public DateOnly DataIni { get; set; }

        public int Duracao { get; set; }

        public decimal Imposto { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = null!;

        [Required]
        public decimal MontanteInvestido { get; set; }

        [Required]
        public decimal TaxaJuros { get; set; }
    }
}
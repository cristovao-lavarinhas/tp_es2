using System.ComponentModel.DataAnnotations;

namespace esii.Models
{
    public class DepositoPrazoCreateViewModel
    {
        public int AtivoId { get; set; }

        // Ativofinanceiro
        [Required]
        public DateOnly DataIni { get; set; }

        public int Duracao { get; set; }

        public decimal Imposto { get; set; }

        // Depositoprazo
        [Required]
        public decimal Valor { get; set; }

        [Required]
        public string Banco { get; set; } = null!;

        [Required]
        public string NumConta { get; set; } = null!;

        [Required]
        public string Titulares { get; set; } = null!;

        [Required]
        public decimal TaxaJurosAnual { get; set; }
    }
}
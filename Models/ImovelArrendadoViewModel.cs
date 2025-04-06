using System.ComponentModel.DataAnnotations;

namespace esii.Models
{
    public class ImovelArrendadoCreateViewModel
    {
        public int AtivoId { get; set; } // necessário para Edit e Delete
        
        // Campos do Ativofinanceiro
        [Required]
        public DateOnly DataIni { get; set; }

        public int Duracao { get; set; }

        public decimal Imposto { get; set; }

        // Campos do Imovelarrendado
        [Required]
        public string Designacao { get; set; } = null!;

        [Required]
        public string Localizacao { get; set; } = null!;

        [Required]
        public decimal ValorImovel { get; set; }

        public decimal ValorRenda { get; set; }

        public decimal ValorMensalCondominio { get; set; }

        public decimal ValorAnualDespesas { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace esii.Models
{
    public class TaxaJuroMensalViewModel
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 12)]
        public int Mes { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Ano { get; set; }

        [Required]
        public decimal Taxa { get; set; }

        public string? Descricao { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace esii.Models;

public class JurosCompostosViewModel
{
    [Required]
    [Range(0.01, 1000000)]
    public decimal CapitalInicial { get; set; }

    [Required]
    [Range(0.01, 100)]
    public decimal TaxaJuros { get; set; }

    [Required]
    [Range(1, 100)]
    public int Periodos { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace esii.Models
{
    public class utilizadorviewmodel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email não é válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "A password é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "O NIF é obrigatório.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O NIF deve ter exatamente 9 dígitos.")]
        public string Nif { get; set; } = null!;
    }
}
using System.ComponentModel.DataAnnotations;

namespace esii.Models;

public class loginviewmodel
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    
    public string Metodo { get; set; } = "password";

    public string? Pin { get; set; }

    public string? OTPCode { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Email))
        {
            yield return new ValidationResult("O email é obrigatório.", new[] { nameof(Email) });
        }

        switch (Metodo?.ToLower())
        {
            case "password":
                if (string.IsNullOrEmpty(Password))
                    yield return new ValidationResult("A password é obrigatória.", new[] { nameof(Password) });
                break;

            case "pin":
                if (string.IsNullOrEmpty(Pin))
                    yield return new ValidationResult("O PIN é obrigatório.", new[] { nameof(Pin) });
                break;

            case "otp":
                if (string.IsNullOrEmpty(OTPCode))
                    yield return new ValidationResult("O código OTP é obrigatório.", new[] { nameof(OTPCode) });
                break;

            default:
                yield return new ValidationResult("Método de login inválido.", new[] { nameof(Metodo) });
                break;
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using esii.Context;
using esii.Models;
using System.Security.Cryptography;
using System.Text;

namespace esii.stratagies.Email
{
    public class OTPLoginStrategy : ILoginStrategy
    {
        private readonly MyDbContext _dbContext;

        public OTPLoginStrategy(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> LoginAsync(HttpContext httpContext, loginviewmodel viewModel)
        {
            var user = await _dbContext.Utilizadors.FirstOrDefaultAsync(u => u.Email == viewModel.Email);
            if (user == null || string.IsNullOrEmpty(viewModel.OTPCode))
                return false;

            // Hashear o código inserido pelo utilizador
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(viewModel.OTPCode.Trim());
            var inputHash = Convert.ToBase64String(sha256.ComputeHash(inputBytes));

            // Comparar com o hash armazenado na base de dados
            if (!string.Equals(user.OTPCode, inputHash))
                return false;

            if (user.OTPExpiry == null || user.OTPExpiry < DateTime.UtcNow)
                return false;

            // Código válido: limpa o OTP e autentica
            user.OTPCode = null;
            user.OTPExpiry = null;
            await _dbContext.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
        }

    }
}

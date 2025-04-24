using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using esii.Context;
using esii.Entities;
using esii.Models;

namespace esii.stratagies.Pin
{
    public class PinLoginStrategy : ILoginStrategy
    {
        private readonly MyDbContext _context;

        public PinLoginStrategy(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(HttpContext httpContext, loginviewmodel viewModel)
        {
            var user = await _context.Utilizadors.FirstOrDefaultAsync(u => u.Email == viewModel.Email);
            if (user == null || string.IsNullOrEmpty(viewModel.Pin))
                return false;

            // Hashear o PIN inserido pelo utilizador
            using var sha256 = SHA256.Create();
            var pinBytes = Encoding.UTF8.GetBytes(viewModel.Pin.Trim());
            var pinHash = Convert.ToBase64String(sha256.ComputeHash(pinBytes));

            // Comparar com o PIN hashado na base de dados
            if (!string.Equals(user.Pin, pinHash))
                return false;

            await SignInUser(httpContext, user);
            return true;
        }

        private async Task SignInUser(HttpContext context, Utilizador user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
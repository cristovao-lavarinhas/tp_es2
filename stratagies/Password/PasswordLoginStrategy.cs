using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using esii.Context;
using esii.Entities;
using esii.Models;

namespace esii.stratagies.Password
{
    public class PasswordLoginStrategy : ILoginStrategy
    {
        private readonly MyDbContext _context;
        private readonly PasswordHasher<Utilizador> _hasher;

        public PasswordLoginStrategy(MyDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<Utilizador>();
        }

        public async Task<bool> LoginAsync(HttpContext httpContext, loginviewmodel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Password))
            {
                return false;
            }

            var user = await _context.Utilizadors.FirstOrDefaultAsync(u => u.Email == viewModel.Email);

            if (user == null)
                return false;

            var result = _hasher.VerifyHashedPassword(user, user.Password, viewModel.Password);
            if (result != PasswordVerificationResult.Success)
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
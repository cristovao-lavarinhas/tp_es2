using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using esii.Context;
using esii.Entities;
using esii.Models;

namespace esii.Controllers
{
    public class UtilizadorController : Controller
    {
        private readonly MyDbContext dbContext;
        private readonly PasswordHasher<Utilizador> passwordHasher;

        public UtilizadorController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.passwordHasher = new PasswordHasher<Utilizador>();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(utilizadorviewmodel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var utilizador = new Utilizador
            {
                Nome = viewModel.Nome,
                Email = viewModel.Email,
                Nif = viewModel.Nif,
                Imposto = 0,
                TipoId = 1
            };

            // Hash da password
            utilizador.Password = passwordHasher.HashPassword(utilizador, viewModel.Password);

            await dbContext.Utilizadors.AddAsync(utilizador);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(loginviewmodel viewModel)
        {
            var utilizador = await dbContext.Utilizadors
                .FirstOrDefaultAsync(u => u.Email == viewModel.Email);

            if (utilizador != null)
            {
                var result = passwordHasher.VerifyHashedPassword(utilizador, utilizador.Password, viewModel.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, utilizador.Id.ToString()),
                        new Claim(ClaimTypes.Name, utilizador.Nome),
                        new Claim(ClaimTypes.Email, utilizador.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true
                        });

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Email ou senha incorretos.");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Utilizador");
        }
    }
}

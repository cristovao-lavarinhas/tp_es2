using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using esii.Context;
using esii.Entities;
using esii.Models;

namespace esii.Controllers
{
    public class UtilizadorController : Controller
    {
        private readonly MyDbContext dbContext;

        public UtilizadorController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(utilizadorviewmodel viewModel)
        {
            var utilizador = new Utilizador
            {
                Nome = viewModel.Nome,
                Email = viewModel.Email,
                Password = viewModel.Password,
                Nif = viewModel.Nif,
                Imposto = 0,
                TipoId = 1
            };

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
                .FirstOrDefaultAsync(u => u.Email == viewModel.Email && u.Password == viewModel.Password);

            if (utilizador != null)
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


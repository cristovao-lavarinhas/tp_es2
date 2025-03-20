using esii.Context;
using esii.Entities;
using esii.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // Biblioteca para hashing seguro de senhas

namespace esii.Controllers
{
    public class utilizadorcontroller : Controller
    {
        private readonly MyDbContext dbContext;

        public utilizadorcontroller(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        [HttpGet]
        // Tela de Registro
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(utilizadorviewmodel viewModel)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(viewModel.Password); // Hash da senha

            var utilizador = new Utilizador
            {
                Nome = viewModel.Nome,
                Email = viewModel.Email,
                Password = hashedPassword, // Salva a senha com hash
                Nif = viewModel.Nif,
                Imposto = 0,
                TipoId = 1
            };

            await dbContext.Utilizadors.AddAsync(utilizador);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home"); // Redireciona para uma página de dashboard
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

            if (utilizador != null && BCrypt.Net.BCrypt.Verify(viewModel.Password, utilizador.Password))
            {
                // Simula a criação de uma sessão para o utilizador
                return RedirectToAction("Index", "Home"); // Redireciona para uma página de dashboard
            }

            ModelState.AddModelError("", "Email ou senha incorretos.");
            return View(viewModel);
        }
    }
}
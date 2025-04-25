using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using esii.Context;
using esii.Entities;
using esii.Models;
using esii.stratagies;
using esii.stratagies.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using IEmailSender = esii.stratagies.Email.IEmailSender;

namespace esii.Controllers
{
    public class UtilizadorController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly PasswordHasher<Utilizador> passwordHasher;
        private readonly LoginContext _loginContext;
        private readonly IEmailSender _emailSender;

        public UtilizadorController(MyDbContext dbContext, LoginContext loginContext, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            passwordHasher = new PasswordHasher<Utilizador>();
            _loginContext = loginContext;
            _emailSender = emailSender;
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
                Pin = viewModel.Pin,
                Imposto = 0,
                TipoId = 1
            };

            // Hash da password
            utilizador.Password = passwordHasher.HashPassword(utilizador, viewModel.Password);

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var pinBytes = System.Text.Encoding.UTF8.GetBytes(viewModel.Pin);
                var pinHashBytes = sha256.ComputeHash(pinBytes);
                utilizador.Pin = Convert.ToBase64String(pinHashBytes);  // Store hashed PIN
            }
            
            await _dbContext.Utilizadors.AddAsync(utilizador);
            await _dbContext.SaveChangesAsync();

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
            
            var strategy = _loginContext.GetStrategy(viewModel.Metodo);
    
            if (strategy == null || !await strategy.LoginAsync(HttpContext, viewModel))
            {
                ModelState.AddModelError("", "Código inválido, expirado ou credenciais incorretas.");
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public async Task<IActionResult> SendOtpCode(string email)
        {
            var user = await _dbContext.Utilizadors.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("Utilizador não encontrado.");
            }

            // Geração de OTP
            var otpCode = new Random().Next(100000, 999999).ToString();
    
            // Hash do código OTP antes de armazenar
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otpCode));
            var hashedOtp = Convert.ToBase64String(hashBytes);

            user.OTPCode = hashedOtp;
            user.OTPExpiry = DateTime.UtcNow.AddMinutes(5);

            await _dbContext.SaveChangesAsync();

            var message = $"Seu código OTP é: {otpCode}. Ele expira em 5 minutos.";

            await _emailSender.SendEmailAsync(user.Email, "Código OTP", message);
            return Ok();
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

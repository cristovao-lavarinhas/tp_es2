using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using esii.Context;
using esii.Entities;

namespace esii.Controllers
{
    [Route("perfil")]
    public class PerfilController : Controller
    {
        private readonly MyDbContext _context;

        public PerfilController(MyDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet("")]
        public async Task<IActionResult> Perfil()
        {
            var id = GetUserId();
            var user = await _context.Utilizadors
                .Include(u => u.Tipo)                          
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user); 
        }

        [HttpPost("alterar-email")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarEmail(string novoEmail, string senhaAtual)
        {
            var id = GetUserId();
            var user = await _context.Utilizadors.FindAsync(id);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(novoEmail))
            {
                TempData["ErroEmail"] = "O email não pode estar vazio.";
                TempData["AbrirModal"] = "email";
                return RedirectToAction(nameof(Perfil));
            }

            if (novoEmail == user.Email)
            {
                TempData["ErroEmail"] = "O novo email é igual ao atual.";
                TempData["AbrirModal"] = "email";
                return RedirectToAction(nameof(Perfil));
            }

            var passwordHasher = new PasswordHasher<Utilizador>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, senhaAtual);
            if (result == PasswordVerificationResult.Failed)
            {
                TempData["ErroEmail"] = "Palavra-passe atual incorreta.";
                TempData["AbrirModal"] = "email";
                return RedirectToAction(nameof(Perfil));
            }

            user.Email = novoEmail;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Perfil));
        }

        [HttpPost("alterar-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarPassword(string passwordAtual, string novaPassword, string confirmarPassword)
        {
            var id = GetUserId();
            var user = await _context.Utilizadors.FindAsync(id);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(novaPassword) || string.IsNullOrWhiteSpace(confirmarPassword))
            {
                TempData["ErroPassword"] = "Preencha todos os campos.";
                TempData["AbrirModal"] = "password";
                return RedirectToAction(nameof(Perfil));
            }

            if (novaPassword != confirmarPassword)
            {
                TempData["ErroPassword"] = "As palavras-passe não coincidem.";
                TempData["AbrirModal"] = "password";
                return RedirectToAction(nameof(Perfil));
            }

            var passwordHasher = new PasswordHasher<Utilizador>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, passwordAtual);
            if (result == PasswordVerificationResult.Failed)
            {
                TempData["ErroPassword"] = "Palavra-passe atual incorreta.";
                TempData["AbrirModal"] = "password";
                return RedirectToAction(nameof(Perfil));
            }

            // nova igual à atual
            var resultNova = passwordHasher.VerifyHashedPassword(user, user.Password, novaPassword);
            if (resultNova == PasswordVerificationResult.Success)
            {
                TempData["ErroPassword"] = "A nova palavra-passe é igual à atual.";
                TempData["AbrirModal"] = "password";
                return RedirectToAction(nameof(Perfil));
            }

            user.Password = passwordHasher.HashPassword(user, novaPassword);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Perfil));
        }


        [HttpGet("eliminar-conta")]
        public async Task<IActionResult> DeleteAccount()
        {
            var id = GetUserId();
            var user = await _context.Utilizadors.FindAsync(id);
            if (user == null) return NotFound();
            return View(user); // View: DeleteAccount.cshtml
        }

        [HttpPost("eliminar-conta")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirm()
        {
            var id = GetUserId();
            var user = await _context.Utilizadors.FindAsync(id);
            if (user == null) return NotFound();

            _context.Utilizadors.Remove(user);
            await _context.SaveChangesAsync();

            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

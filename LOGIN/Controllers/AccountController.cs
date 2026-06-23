using Microsoft.AspNetCore.Mvc;
using LOGIN.Data;
using LOGIN.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace LOGIN.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string Email, string Contrasena, bool Recordarme)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
            {
                ViewData.ModelState.AddModelError("", "Email y contraseña son obligatorios");
                return View();
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == Email);

            if (usuario == null)
            {
                ViewData.ModelState.AddModelError("", "Email o contraseña incorrectos");
                return View();
            }

            if (!VerificarContrasena(Contrasena, usuario.ContrasenaHash))
            {
                ViewData.ModelState.AddModelError("", "Email o contraseña incorrectos");
                return View();
            }

            // ⭐ SIN ROL - Solo guardamos lo básico
            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre ?? "Usuario");
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email ?? "");

            TempData["Success"] = $"¡Bienvenido de nuevo, {usuario.Nombre}!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Register
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(string Nombre, string Email, string Contrasena, string ConfirmarContrasena)
        {
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
            {
                ViewData.ModelState.AddModelError("", "Todos los campos son obligatorios");
                return View();
            }

            if (Contrasena != ConfirmarContrasena)
            {
                ViewData.ModelState.AddModelError("", "Las contraseñas no coinciden");
                return View();
            }

            if (Contrasena.Length < 6)
            {
                ViewData.ModelState.AddModelError("", "La contraseña debe tener al menos 6 caracteres");
                return View();
            }

            if (_context.Usuarios.Any(u => u.Email == Email))
            {
                ViewData.ModelState.AddModelError("", "Este email ya está registrado");
                return View();
            }

            var usuario = new Usuario
            {
                Nombre = Nombre,
                Email = Email,
                ContrasenaHash = HashContrasena(Contrasena),
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["Mensaje"] = "¡Registro exitoso! Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Mensaje"] = "Has cerrado sesión correctamente";
            return RedirectToAction("Login", "Account");
        }

        private string HashContrasena(string contrasena)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(contrasena);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerificarContrasena(string contrasena, string hash)
        {
            return HashContrasena(contrasena) == hash;
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace LOGIN.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? ContrasenaHash { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // ⭐ SIN ROL - Eliminado
    }
}
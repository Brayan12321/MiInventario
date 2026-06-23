using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOGIN.Models
{
    public class CarritoItem
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto? Producto { get; set; }
    }
}
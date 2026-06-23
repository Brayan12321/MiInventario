using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOGIN.Models
{
    public enum EstadoPedido
    {
        Confirmado,
        Pendiente,
        Enviado,
        Entregado,
        Cancelado
    }

    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public decimal Total { get; set; }

        public string? DireccionEnvio { get; set; }

        public string? MetodoPago { get; set; }

        public string? NumeroReferencia { get; set; }

        public EstadoPedido Estado { get; set; } = EstadoPedido.Confirmado;

        public DateTime FechaPedido { get; set; } = DateTime.Now;

        public virtual ICollection<PedidoDetalle>? Detalles { get; set; }
    }
}
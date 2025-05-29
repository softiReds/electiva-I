namespace ElectivaI.Data.Models;

public partial class Orden
{
    public int IdOrden { get; set; }

    public int IdPersona { get; set; }

    public int IdProducto { get; set; }

    public DateTime FechaOrden { get; set; }

    public int CantidadOrden { get; set; }

    public decimal TotalOrden { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}

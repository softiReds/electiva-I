namespace ElectivaI.Data.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoria { get; set; }

    public int IdEmpleado { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string? DescripcionProducto { get; set; }

    public decimal PrecioProducto { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();
}

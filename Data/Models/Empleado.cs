namespace ElectivaI.Data.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public int IdPersona { get; set; }

    public int IdTipoEmpleado { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual TipoEmpleado IdTipoEmpleadoNavigation { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}

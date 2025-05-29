namespace ElectivaI.Data.Models;

public class TipoEmpleado
{
    public int IdTipoEmpleado { get; set; }

    public string TipoEmpleado1 { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
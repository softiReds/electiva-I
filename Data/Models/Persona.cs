namespace ElectivaI.Data.Models;

public partial class Persona
{
    public int IdPersona { get; set; }

    public int IdRol { get; set; }

    public string NombrePersona { get; set; } = null!;

    public string ApellidoPersona { get; set; } = null!;

    public string CorreoPersona { get; set; } = null!;

    public string? TelefonoPersona { get; set; }

    public string? DireccionPersona { get; set; }

    public string ContrasenaPersona { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();
}

using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de roles
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RolController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de roles
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public RolController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todos los roles
    /// </summary>
    /// <returns>Lista de todos los roles</returns>
    /// <response code="200">Retorna la lista de roles</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var roles = _dbContext.Rols.ToList();
        return Ok(roles);
    }

    /// <summary>
    /// Obtiene un rol por su ID
    /// </summary>
    /// <param name="id">ID del rol a buscar</param>
    /// <returns>El rol solicitado</returns>
    /// <response code="200">Retorna el rol encontrado</response>
    /// <response code="404">Si el rol no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var rol = _dbContext.Rols.Find(id);
        if (rol == null)
            return NotFound($"No se encontró un rol con ID {id}.");

        return Ok(rol);
    }

    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    /// <param name="rol">Datos del rol a crear</param>
    /// <returns>El rol creado</returns>
    /// <response code="201">Retorna el rol creado</response>
    /// <response code="400">Si los datos del rol son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Rol rol)
    {
        _dbContext.Rols.Add(rol);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = rol.IdRol }, rol);
    }

    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    /// <param name="id">ID del rol a actualizar</param>
    /// <param name="rol">Nuevos datos del rol</param>
    /// <returns>No Content si la actualización es exitosa</returns>
    /// <response code="204">Si el rol fue actualizado correctamente</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si el rol no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] Rol rol)
    {
        if (id != rol.IdRol)
            return BadRequest("Los datos enviados no coinciden con el ID.");

        var existing = _dbContext.Rols.Find(id);
        if (existing == null)
            return NotFound($"No se encontró un rol con ID {id}.");

        existing.NombreRol = rol.NombreRol;
        _dbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Elimina un rol existente del sistema
    /// </summary>
    /// <param name="id">Identificador único del rol que se desea eliminar</param>
    /// <returns>
    /// - NoContent (204): Si el rol fue eliminado exitosamente
    /// - NotFound (404): Si no se encuentra un rol con el ID especificado
    /// </returns>
    /// <remarks>
    /// Esta operación es irreversible. Una vez eliminado el rol, no podrá ser recuperado.
    /// Se debe tener precaución al eliminar roles que estén siendo utilizados por usuarios del sistema.
    /// 
    /// Ejemplo de uso:
    /// 
    ///     DELETE /api/rol/5
    /// 
    /// </remarks>
    /// <response code="204">El rol fue eliminado exitosamente</response>
    /// <response code="404">No se encontró el rol con el ID especificado</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var rol = _dbContext.Rols.Find(id);
        if (rol == null)
            return NotFound($"No se encontró un rol con ID {id}.");

        _dbContext.Rols.Remove(rol);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
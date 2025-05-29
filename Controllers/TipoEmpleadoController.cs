using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de tipos de empleado
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TipoEmpleadoController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de tipos de empleado
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public TipoEmpleadoController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todos los tipos de empleado
    /// </summary>
    /// <returns>Lista de todos los tipos de empleado</returns>
    /// <response code="200">Retorna la lista de tipos de empleado</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var tiposEmpleado = _dbContext.TipoEmpleados.ToList();
        return Ok(tiposEmpleado);
    }

    /// <summary>
    /// Obtiene un tipo de empleado por su ID
    /// </summary>
    /// <param name="id">ID del tipo de empleado a buscar</param>
    /// <returns>El tipo de empleado solicitado</returns>
    /// <response code="200">Retorna el tipo de empleado encontrado</response>
    /// <response code="404">Si el tipo de empleado no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var tipoEmpleado = _dbContext.TipoEmpleados.Find(id);
        if (tipoEmpleado == null)
            return NotFound($"No se encontró un tipo de empleado con ID {id}.");

        return Ok(tipoEmpleado);
    }

    /// <summary>
    /// Crea un nuevo tipo de empleado
    /// </summary>
    /// <param name="tipoEmpleado">Datos del tipo de empleado a crear</param>
    /// <returns>El tipo de empleado creado</returns>
    /// <response code="201">Retorna el tipo de empleado creado</response>
    /// <response code="400">Si los datos del tipo de empleado son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TipoEmpleado tipoEmpleado)
    {
        _dbContext.TipoEmpleados.Add(tipoEmpleado);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = tipoEmpleado.IdTipoEmpleado }, tipoEmpleado);
    }

    /// <summary>
    /// Actualiza un tipo de empleado existente
    /// </summary>
    /// <param name="id">ID del tipo de empleado a actualizar</param>
    /// <param name="tipoEmpleado">Nuevos datos del tipo de empleado</param>
    /// <returns>No Content si la actualización es exitosa</returns>
    /// <response code="204">Si el tipo de empleado fue actualizado correctamente</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si el tipo de empleado no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] TipoEmpleado tipoEmpleado)
    {
        if (id != tipoEmpleado.IdTipoEmpleado)
            return BadRequest("Los datos enviados no coinciden con el ID.");

        var existing = _dbContext.TipoEmpleados.Find(id);
        if (existing == null)
            return NotFound($"No se encontró un tipo de empleado con ID {id}.");

        existing.TipoEmpleado1 = tipoEmpleado.TipoEmpleado1;
        _dbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Elimina un tipo de empleado
    /// </summary>
    /// <param name="id">ID del tipo de empleado a eliminar</param>
    /// <returns>No Content si la eliminación es exitosa</returns>
    /// <response code="204">Si el tipo de empleado fue eliminado correctamente</response>
    /// <response code="404">Si el tipo de empleado no existe</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var tipoEmpleado = _dbContext.TipoEmpleados.Find(id);
        if (tipoEmpleado == null)
            return NotFound($"No se encontró un tipo de empleado con ID {id}.");

        _dbContext.TipoEmpleados.Remove(tipoEmpleado);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
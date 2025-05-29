using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de empleados
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EmpleadoController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de empleados
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public EmpleadoController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todos los empleados
    /// </summary>
    /// <returns>Lista de todos los empleados</returns>
    /// <response code="200">Retorna la lista de empleados</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var empleados = _dbContext.Empleados.ToList();
        return Ok(empleados);
    }

    /// <summary>
    /// Obtiene un empleado por su ID
    /// </summary>
    /// <param name="id">ID del empleado a buscar</param>
    /// <returns>El empleado solicitado</returns>
    /// <response code="200">Retorna el empleado encontrado</response>
    /// <response code="404">Si el empleado no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var empleado = _dbContext.Empleados.Find(id);
        if (empleado == null)
            return NotFound($"No se encontró un empleado con ID {id}.");

        return Ok(empleado);
    }

    /// <summary>
    /// Crea un nuevo empleado
    /// </summary>
    /// <param name="empleado">Datos del empleado a crear</param>
    /// <returns>El empleado creado</returns>
    /// <response code="201">Retorna el empleado creado</response>
    /// <response code="400">Si los datos del empleado son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Empleado empleado)
    {
        _dbContext.Empleados.Add(empleado);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = empleado.IdEmpleado }, empleado);
    }

    /// <summary>
    /// Actualiza un empleado existente
    /// </summary>
    /// <param name="id">ID del empleado a actualizar</param>
    /// <param name="empleado">Nuevos datos del empleado</param>
    /// <returns>El empleado actualizado</returns>
    /// <response code="200">Retorna el empleado actualizado</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si el empleado no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Update(int id, [FromBody] Empleado empleado)
    {
        if (empleado.IdEmpleado != id)
            return BadRequest("Los datos del empleado son inválidos.");

        var existingEmpleado = _dbContext.Empleados.Find(id);
        if (existingEmpleado == null)
            return NotFound($"No se encontró un empleado con ID {id}.");

        existingEmpleado.IdPersona = empleado.IdPersona;
        existingEmpleado.IdTipoEmpleado = empleado.IdTipoEmpleado;

        _dbContext.SaveChanges();

        return Ok(existingEmpleado);
    }

    /// <summary>
    /// Elimina un empleado
    /// </summary>
    /// <param name="id">ID del empleado a eliminar</param>
    /// <returns>No Content si la eliminación es exitosa</returns>
    /// <response code="204">Si el empleado fue eliminado correctamente</response>
    /// <response code="404">Si el empleado no existe</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var empleado = _dbContext.Empleados.Find(id);
        if (empleado == null)
            return NotFound($"No se encontró un empleado con ID {id}.");

        _dbContext.Empleados.Remove(empleado);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
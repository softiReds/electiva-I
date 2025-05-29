using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de personas
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PersonaController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de personas
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public PersonaController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todas las personas
    /// </summary>
    /// <returns>Lista de todas las personas</returns>
    /// <response code="200">Retorna la lista de personas</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var personas = _dbContext.Personas.ToList();
        return Ok(personas);
    }

    /// <summary>
    /// Obtiene una persona por su ID
    /// </summary>
    /// <param name="id">ID de la persona a buscar</param>
    /// <returns>La persona solicitada</returns>
    /// <response code="200">Retorna la persona encontrada</response>
    /// <response code="404">Si la persona no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var persona = _dbContext.Personas.Find(id);
        if (persona == null)
            return NotFound($"No se encontró una persona con ID {id}.");

        return Ok(persona);
    }

    /// <summary>
    /// Crea una nueva persona
    /// </summary>
    /// <param name="persona">Datos de la persona a crear</param>
    /// <returns>La persona creada</returns>
    /// <response code="201">Retorna la persona creada</response>
    /// <response code="400">Si los datos de la persona son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Persona persona)
    {
        var rol = _dbContext.Rols.Find(persona.IdRol);
        if (rol == null)
            return BadRequest($"No existe el rol con ID {persona.IdRol}");

        _dbContext.Personas.Add(persona);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = persona.IdPersona }, persona);
    }

    /// <summary>
    /// Actualiza una persona existente
    /// </summary>
    /// <param name="id">ID de la persona a actualizar</param>
    /// <param name="persona">Nuevos datos de la persona</param>
    /// <returns>No Content si la actualización es exitosa</returns>
    /// <response code="204">Si la persona fue actualizada correctamente</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si la persona no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] Persona persona)
    {
        if (id != persona.IdPersona)
            return BadRequest("Los datos enviados no coinciden con el ID.");

        var existing = _dbContext.Personas.Find(id);
        if (existing == null)
            return NotFound($"No se encontró una persona con ID {id}.");

        var rol = _dbContext.Rols.Find(persona.IdRol);
        if (rol == null)
            return BadRequest($"No existe el rol con ID {persona.IdRol}");

        existing.NombrePersona = persona.NombrePersona;
        existing.ApellidoPersona = persona.ApellidoPersona;
        existing.CorreoPersona = persona.CorreoPersona;
        existing.ContrasenaPersona = persona.ContrasenaPersona;
        existing.DireccionPersona = persona.DireccionPersona;
        existing.TelefonoPersona = persona.TelefonoPersona;
        existing.IdRol = persona.IdRol;

        _dbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Elimina una persona
    /// </summary>
    /// <param name="id">ID de la persona a eliminar</param>
    /// <returns>No Content si la eliminación es exitosa</returns>
    /// <response code="204">Si la persona fue eliminada correctamente</response>
    /// <response code="404">Si la persona no existe</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var persona = _dbContext.Personas.Find(id);
        if (persona == null)
            return NotFound($"No se encontró una persona con ID {id}.");

        _dbContext.Personas.Remove(persona);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
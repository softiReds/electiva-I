
using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de inventario
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class InventarioController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de inventario
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public InventarioController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todos los items del inventario
    /// </summary>
    /// <returns>Lista de todos los items del inventario</returns>
    /// <response code="200">Retorna la lista de items del inventario</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var inventario = _dbContext.Inventario.ToList();
        return Ok(inventario);
    }

    /// <summary>
    /// Obtiene un item del inventario por su ID
    /// </summary>
    /// <param name="id">ID del item a buscar</param>
    /// <returns>El item del inventario solicitado</returns>
    /// <response code="200">Retorna el item encontrado</response>
    /// <response code="404">Si el item no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var item = _dbContext.Inventario.Find(id);
        if (item == null)
            return NotFound($"No se encontró un item con ID {id}.");

        return Ok(item);
    }

    /// <summary>
    /// Crea un nuevo item en el inventario
    /// </summary>
    /// <param name="inventario">Datos del item a crear</param>
    /// <returns>El item creado</returns>
    /// <response code="201">Retorna el item creado</response>
    /// <response code="400">Si los datos del item son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Inventario inventario)
    {
        _dbContext.Inventario.Add(inventario);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = inventario.IdInventario }, inventario);
    }

    /// <summary>
    /// Actualiza un item existente del inventario
    /// </summary>
    /// <param name="id">ID del item a actualizar</param>
    /// <param name="inventario">Nuevos datos del item</param>
    /// <returns>No Content si la actualización es exitosa</returns>
    /// <response code="204">Si el item fue actualizado correctamente</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si el item no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] Inventario inventario)
    {
        if (id != inventario.IdInventario)
            return BadRequest("Los datos enviados no coinciden con el ID.");

        var existing = _dbContext.Inventario.Find(id);
        if (existing == null)
            return NotFound($"No se encontró un item con ID {id}.");

        existing.IdProducto = inventario.IdProducto;
        existing.IdEmpleado = inventario.IdEmpleado;
        existing.CantidadInventario = inventario.CantidadInventario;
        _dbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Elimina un item del inventario
    /// </summary>
    /// <param name="id">ID del item a eliminar</param>
    /// <returns>No Content si la eliminación es exitosa</returns>
    /// <response code="204">Si el item fue eliminado correctamente</response>
    /// <response code="404">Si el item no existe</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var item = _dbContext.Inventario.Find(id);
        if (item == null)
            return NotFound($"No se encontró un item con ID {id}.");

        _dbContext.Inventario.Remove(item);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
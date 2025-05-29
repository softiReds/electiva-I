using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de órdenes
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrdenController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de órdenes
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public OrdenController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todas las órdenes
    /// </summary>
    /// <returns>Lista de todas las órdenes</returns>
    /// <response code="200">Retorna la lista de órdenes</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var ordenes = _dbContext.Ordens.ToList();
        return Ok(ordenes);
    }

    /// <summary>
    /// Obtiene una orden por su ID
    /// </summary>
    /// <param name="id">ID de la orden a buscar</param>
    /// <returns>La orden solicitada</returns>
    /// <response code="200">Retorna la orden encontrada</response>
    /// <response code="404">Si la orden no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var orden = _dbContext.Ordens.Find(id);

        if (orden == null)
            return NotFound($"No se encontró una orden con ID {id}.");

        return Ok(orden);
    }

    /// <summary>
    /// Crea una nueva orden
    /// </summary>
    /// <param name="orden">Datos de la orden a crear</param>
    /// <returns>La orden creada</returns>
    /// <response code="201">Retorna la orden creada</response>
    /// <response code="400">Si los datos de la orden son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Orden orden)
    {
        var producto = _dbContext.Productos.Find(orden.IdProducto);
        if (producto == null)
            return BadRequest($"No existe el producto con ID {orden.IdProducto}");

        var persona = _dbContext.Personas.Find(orden.IdPersona);
        if (persona == null)
            return BadRequest($"No existe la persona con ID {orden.IdPersona}");

        orden.TotalOrden = orden.CantidadOrden * producto.PrecioProducto;
        orden.FechaOrden = DateTime.Now;

        _dbContext.Ordens.Add(orden);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = orden.IdOrden }, orden);
    }

    /// <summary>
    /// Actualiza una orden existente
    /// </summary>
    /// <param name="id">ID de la orden a actualizar</param>
    /// <param name="orden">Nuevos datos de la orden</param>
    /// <returns>No Content si la actualización es exitosa</returns>
    /// <response code="204">Si la orden fue actualizada correctamente</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si la orden no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] Orden orden)
    {
        if (id != orden.IdOrden)
            return BadRequest("Los datos enviados no coinciden con el ID.");

        var existing = _dbContext.Ordens.Find(id);
        if (existing == null)
            return NotFound($"No se encontró una orden con ID {id}.");

        var producto = _dbContext.Productos.Find(orden.IdProducto);
        if (producto == null)
            return BadRequest($"No existe el producto con ID {orden.IdProducto}");

        existing.IdPersona = orden.IdPersona;
        existing.IdProducto = orden.IdProducto;
        existing.FechaOrden = orden.FechaOrden;
        existing.CantidadOrden = orden.CantidadOrden;
        existing.TotalOrden = orden.CantidadOrden * producto.PrecioProducto;

        _dbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Elimina una orden existente del sistema
    /// </summary>
    /// <param name="id">Identificador único de la orden que se desea eliminar</param>
    /// <returns>
    /// - NoContent (204): Si la orden fue eliminada exitosamente
    /// - NotFound (404): Si no se encuentra una orden con el ID especificado
    /// </returns>
    /// <remarks>
    /// Esta operación es irreversible. Una vez eliminada la orden, no podrá ser recuperada.
    /// Ejemplo de uso:
    /// 
    ///     DELETE /api/orden/5
    /// 
    /// </remarks>
    /// <response code="204">La orden fue eliminada exitosamente</response>
    /// <response code="404">No se encontró la orden con el ID especificado</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var orden = _dbContext.Ordens.Find(id);
        if (orden == null)
            return NotFound($"No se encontró una orden con ID {id}.");

        _dbContext.Ordens.Remove(orden);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
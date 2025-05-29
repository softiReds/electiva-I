using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de categorías
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de categorías
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public CategoriaController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todas las categorías
    /// </summary>
    /// <returns>Lista de todas las categorías</returns>
    /// <response code="200">Retorna la lista de categorías</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var categorias = _dbContext.Categoria.ToList();
        return Ok(categorias);
    }

    /// <summary>
    /// Obtiene una categoría por su ID
    /// </summary>
    /// <param name="id">ID de la categoría a buscar</param>
    /// <returns>La categoría solicitada</returns>
    /// <response code="200">Retorna la categoría encontrada</response>
    /// <response code="404">Si la categoría no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var categoria = _dbContext.Categoria.Find(id);
        if (categoria == null)
            return NotFound($"No se encontró una categoría con ID {id}.");

        return Ok(categoria);
    }

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    /// <param name="categoria">Datos de la categoría a crear</param>
    /// <returns>La categoría creada</returns>
    /// <response code="201">Retorna la categoría creada</response>
    /// <response code="400">Si los datos de la categoría son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Categoria categoria)
    {
        _dbContext.Categoria.Add(categoria);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = categoria.IdCategoria }, categoria);
    }

    /// <summary>
    /// Actualiza una categoría existente
    /// </summary>
    /// <param name="id">ID de la categoría a actualizar</param>
    /// <param name="categoria">Nuevos datos de la categoría</param>
    /// <returns>No Content si la actualización es exitosa</returns>
    /// <response code="204">Si la categoría fue actualizada correctamente</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si la categoría no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] Categoria categoria)
    {
        if (id != categoria.IdCategoria)
            return BadRequest("Los datos enviados no coinciden con el ID.");

        var existing = _dbContext.Categoria.Find(id);
        if (existing == null)
            return NotFound($"No se encontró una categoría con ID {id}.");

        existing.NombreCategoria = categoria.NombreCategoria;
        _dbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Elimina una categoría
    /// </summary>
    /// <param name="id">ID de la categoría a eliminar</param>
    /// <returns>No Content si la eliminación es exitosa</returns>
    /// <response code="204">Si la categoría fue eliminada correctamente</response>
    /// <response code="404">Si la categoría no existe</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var categoria = _dbContext.Categoria.Find(id);
        if (categoria == null)
            return NotFound($"No se encontró una categoría con ID {id}.");

        _dbContext.Categoria.Remove(categoria);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
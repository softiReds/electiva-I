using ElectivaI.Data.Context;
using ElectivaI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectivaI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de productos
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private readonly ElectivaIContext _dbContext;

    /// <summary>
    /// Constructor del controlador de productos
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos</param>
    public ProductoController(ElectivaIContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todos los productos
    /// </summary>
    /// <returns>Lista de todos los productos</returns>
    /// <response code="200">Retorna la lista de productos</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var productos = _dbContext.Productos.ToList();
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto por su ID
    /// </summary>
    /// <param name="id">ID del producto a buscar</param>
    /// <returns>El producto solicitado</returns>
    /// <response code="200">Retorna el producto encontrado</response>
    /// <response code="404">Si el producto no existe</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var producto = _dbContext.Productos.Find(id);
        if (producto == null)
            return NotFound($"No se encontró un producto con ID {id}.");

        return Ok(producto);
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    /// <param name="producto">Datos del producto a crear</param>
    /// <returns>El producto creado</returns>
    /// <response code="201">Retorna el producto creado</response>
    /// <response code="400">Si los datos del producto son inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Producto producto)
    {
        var categoria = _dbContext.Categoria.Find(producto.IdCategoria);
        if (categoria == null)
            return BadRequest($"No existe la categoría con ID {producto.IdCategoria}");

        var empleado = _dbContext.Empleados.Find(producto.IdEmpleado);
        if (empleado == null)
            return BadRequest($"No existe el empleado con ID {producto.IdEmpleado}");

        if (producto.PrecioProducto <= 0)
            return BadRequest("El precio del producto debe ser mayor que 0");

        _dbContext.Productos.Add(producto);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = producto.IdProducto }, producto);
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    /// <param name="id">ID del producto a actualizar</param>
    /// <param name="producto">Nuevos datos del producto</param>
    /// <returns>No Content si la actualización es exitosa</returns>
    /// <response code="204">Si el producto fue actualizado correctamente</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="404">Si el producto no existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] Producto producto)
    {
        if (id != producto.IdProducto)
            return BadRequest("Los datos enviados no coinciden con el ID.");

        var existing = _dbContext.Productos.Find(id);
        if (existing == null)
            return NotFound($"No se encontró un producto con ID {id}.");

        var categoria = _dbContext.Categoria.Find(producto.IdCategoria);
        if (categoria == null)
            return BadRequest($"No existe la categoría con ID {producto.IdCategoria}");

        var empleado = _dbContext.Empleados.Find(producto.IdEmpleado);
        if (empleado == null)
            return BadRequest($"No existe el empleado con ID {producto.IdEmpleado}");

        if (producto.PrecioProducto <= 0)
            return BadRequest("El precio del producto debe ser mayor que 0");

        existing.NombreProducto = producto.NombreProducto;
        existing.DescripcionProducto = producto.DescripcionProducto;
        existing.PrecioProducto = producto.PrecioProducto;
        existing.IdCategoria = producto.IdCategoria;
        existing.IdEmpleado = producto.IdEmpleado;

        _dbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Elimina un producto
    /// </summary>
    /// <param name="id">ID del producto a eliminar</param>
    /// <returns>No Content si la eliminación es exitosa</returns>
    /// <response code="204">Si el producto fue eliminado correctamente</response>
    /// <response code="404">Si el producto no existe</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var producto = _dbContext.Productos.Find(id);
        if (producto == null)
            return NotFound($"No se encontró un producto con ID {id}.");

        _dbContext.Productos.Remove(producto);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase {

    private readonly TiendaContext _context;

    public ProductosController(TiendaContext context) {
        _context = context;
    }

    [HttpGet]
    // [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)] //Esto es para que salga en la docuemntacion del swagger que tipo de dato y codigos devuelve
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Producto>> Get() {
        return _context.Productos.ToList();
    }

    [HttpGet("{id}", Name = "GetById")]
    public ActionResult<Producto> GetById(int id) {
        var producto = _context.Productos.Find(id);
        if (producto == null) {
            return NotFound();
        }
        return producto;
    }

    [HttpPost]
    public ActionResult Post([FromBody] Producto producto) {

        _context.Productos.Add(producto);
        _context.SaveChanges();
        return new CreatedAtRouteResult("GetById", new { id = producto.Id }, producto);
        // return new CreatedAtRouteResult(nameof(GetById), new { id = producto.Id }, producto);
        // return NoContent();
        // return Ok();
    }


}

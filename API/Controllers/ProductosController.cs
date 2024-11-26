
using API.DTOs;
using API.Helpers;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[ApiVersion("3", Deprecated = true)]
[Route("api/v{version:apiVersion}/[controller]")]
// [Route("api/[controller]")]
// [Authorize(Roles = "Empleado")] //Recuerda que esto funciona porque en el Program.cs se habilito el servicio de app.UseAutorization() y en el JWT se habilito el uso de roles con el ClaimType: "roles"
// [Authorize]
public class ProductosController : ControllerBase {

    //se usa ahora UnitOfWork
    // private readonly IProductoRepository _productRepository;

    // public ProductosController(IProductoRepository productRepository) {
    //     _productRepository = productRepository;
    // }

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductosController> logger) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }


    // [HttpGet]
    // [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)] //Esto es para que salga en la docuemntacion del swagger que tipo de dato y codigos devuelve
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [EnableRateLimiting("fixed")] // esto es para habilitar el rate limiting cuando se usa la libreria nativa de aspnetcore
    // public async Task<ActionResult<List<ProductoListDTO>>> Get() {
    //     // return _context.Productos.ToList();
    //     // var result = await _productRepository.GetAllAsync();
    //     var result = await _unitOfWork.Productosrepository.GetAllAsync();
    //     var resultDTO = _mapper.Map<List<ProductoListDTO>>(result);
    //     return Ok(resultDTO);
    // }

    [HttpGet]
    public async Task<ActionResult<Paginator<ProductoListDTO>>> Get([FromQuery] QueryParams queryParams) {

        var result = await _unitOfWork.Productosrepository.GetAllAsync(queryParams.PageIndex, queryParams.PageSize, queryParams.Search ?? "");
        var resultDTO = _mapper.Map<List<ProductoListDTO>>(result.items);

        var pager = new Paginator<ProductoListDTO>(queryParams.PageIndex, queryParams.PageSize, result.totalItems, resultDTO);

        //ejemplo para responde en los headers
        Response.Headers.Append("x-totalItems", result.totalItems.ToString());
        Response.Headers.Append("x-pageIndex", queryParams.PageIndex.ToString());
        Response.Headers.Append("x-pageSize", queryParams.PageSize.ToString());
        Response.Headers.Append("x-totalPages", pager.TotalPages.ToString());
        Response.Headers.Append("x-hasNext", pager.HasNextPage.ToString());
        Response.Headers.Append("x-hasPrevious", pager.HasPreviousPage.ToString());

        return Ok(pager);

    }

    [HttpGet]
    [MapToApiVersion("2")]
    public async Task<ActionResult<List<ProductoListDTO>>> GetV2() {
        var result = await _unitOfWork.Productosrepository.GetAllAsync();
        var resultDTO = _mapper.Map<List<ProductoListDTO>>(result);

        return Ok(resultDTO.Take(3));
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<ActionResult<Producto>> GetById(int id) {
        // var producto = _context.Productos.Find(id);
        var producto = await _unitOfWork.Productosrepository.GetByIdAsync(id);
        if (producto == null) {
            return NotFound(new ApiResponse(404));
        }
        return producto;
    }

    [HttpPost]
    // public ActionResult Post([FromBody] Producto producto) {
    public async Task<ActionResult<Producto>> Post([FromBody] ProductoDTO productoDto) {

        // _context.Productos.Add(producto);
        // _context.SaveChanges();
         _logger.LogInformation("Creando un nuevo producto, usando serilog"); ///usando serilog

        var producto = _mapper.Map<Producto>(productoDto);

        //se usa un mapper ahora 
        // var producto = new Producto {
        //     Nombre = productoDto.Nombre,
        //     Precio = productoDto.Precio,
        //     FechaCreacion = DateTime.Now,
        //     MarcaId = productoDto.MarcaId,
        //     CategoriaId = productoDto.CategoriaId
        // };

        // _productRepository.Add(producto); //se usa ahora UnitOfWork
        _unitOfWork.Productosrepository.Add(producto);
        await _unitOfWork.Save();
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);


        //Estos ejemplo sond e Felipe Gavilán

        //  pero recuerda colocar el nombre del metodo en el atributo HttpGet(Name="Get")
        // return new CreatedAtRouteResult(nameof(Get), new { id = producto.Id }, producto);
        // otra forma con el nombre del metodo,
        // return NoContent();
        // return new CreatedAtRouteResult("Get", new { id = producto.Id }, producto);
        // return Ok();


        //Erores personalizados
        // var yaExisteUnProuctoConEseNombre = await context.Laptops.AnyAsync(x => x.Nombre == laptop.Nombre);
        // var result = await _productRepository.GetAllAsync();
        var result = await _unitOfWork.Productosrepository.GetAllAsync();
        var yaExisteUnProuctoConEseNombre = result.Any(x => x.Nombre == productoDto.Nombre);

        if (yaExisteUnProuctoConEseNombre) {
            var mensajeDeError = $"Ya existe una laptop con el nombre {productoDto.Nombre!}";
            ModelState.AddModelError(nameof(productoDto.Nombre), mensajeDeError);
            return ValidationProblem(ModelState);
            //Esto devuelve un 400 con el mensaje de error y el sigueinte modelo:
            // {
            //     "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            //     "title": "One or more validation errors occurred.",
            //     "status": 400,
            //     "traceId": "|b3b3b3b3-4b3b-4b3b-4b3b-4b3b3b3b3b3b",
            //     "errors": {
            //         "Nombre": [
            //             "Ya existe una laptop con el nombre Laptop 1"
            //         ]
            //     }
            // }

        }

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Producto>> Put([FromBody] ProductoDTO productoDTO, int id) {

        var producto = await _unitOfWork.Productosrepository.GetByIdAsync(id);
        if (producto == null) {
            return NotFound();
        }

        // producto.Nombre = productoDTO.Nombre;
        // producto.Precio = productoDTO.Precio;
        // producto.MarcaId = productoDTO.MarcaId;
        // producto.CategoriaId = productoDTO.CategoriaId;

        _mapper.Map(productoDTO, producto);


        //Este paso no es necesario porque el objeto ya esta siendo rastreado por EF        
        // _unitOfWork.Productosrepository.Update(producto);

        await _unitOfWork.Save();
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) {
        var producto = await _unitOfWork.Productosrepository.GetByIdAsync(id);
        if (producto == null) {
            return NotFound();
        }

        _unitOfWork.Productosrepository.Remove(producto);
        await _unitOfWork.Save();
        return NoContent();
    }
}



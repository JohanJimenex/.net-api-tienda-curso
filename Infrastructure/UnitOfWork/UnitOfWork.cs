using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

/// <summary>
/// Aqui estamos implementando el patron de diseño UnitOfWork, que nos permite tener un solo punto de acceso a la base de datos
/// y a traves de el acceder a los repositorios de las entidades que necesitemos. 
/// </summary>

public class UnitOfWork : IUnitOfWork, IDisposable {

    private readonly TiendaContext _context;

    private IProductoRepository _productos;
    private ICategoriaRepository _categorias;
    private IMarcaRepository _marcas;

    public IProductoRepository Productosrepository {
        get {
            if (_productos == null) {
                _productos = new ProductoRepositoryImpl(_context);
            }
            return _productos;
        }
    }

    public ICategoriaRepository CategoriasRepository {
        get {
            if (_categorias == null) {
                _categorias = new CategoriaRepositoryImpl(_context);
            }
            return _categorias;
        }
    }

    public IMarcaRepository MarcasRepository {
        get {
            if (_marcas == null) {
                _marcas = new MarcaRepositoryImpl(_context);
            }
            return _marcas;
        }
    }

    public UnitOfWork(TiendaContext context) {
        _context = context;
    }

    public async Task<int> Save() {
        return await _context.SaveChangesAsync();
    }

    public void Dispose() {
        _context.Dispose();
    }
}


//Otra forma de impleemntar UnitOfWork usando injeccion de dependencias en el constructor
//aunque en este caso las instancias se cargan de manera perezosa
public class UnitOfWork2 : IUnitOfWork {

    public IProductoRepository Productosrepository { get; }
    public ICategoriaRepository CategoriasRepository { get; }
    public IMarcaRepository MarcasRepository { get; }

    private readonly TiendaContext _context;

    public UnitOfWork2(IProductoRepository productos, ICategoriaRepository categorias, IMarcaRepository marcas, TiendaContext context) {
        Productosrepository = productos;
        CategoriasRepository = categorias;
        MarcasRepository = marcas;
        _context = context;
    }

    //Guarda los cambios en la base de datos
    public async Task<int> Save() {
        return await _context.SaveChangesAsync();
    }

    //Libera los recursos de la memoria
    public void Dispose() {
        _context.Dispose();
    }
}
using System.Globalization;
using System.Reflection;
using Core.Entities;
using CsvHelper;
using Microsoft.Extensions.Logging;
namespace Infrastructure.data;

// Clase que se encarga de poblar la base de datos con datos de prueba
public class TiendaContextSeed {
    //Metodo para poblar la base de datos
    public static async Task SeedAsync(TiendaContext tiendaContext, ILoggerFactory loggerFactory) {
        try {
            // Los roles deben existir antes de registrar usuarios. Esta validación
            // es idempotente para que también repare una base ya creada sin roles.
            var rolesIniciales = new[] { "Admin", "Gerente", "Empleado" };
            var rolesExistentes = tiendaContext.Roles
                .Where(r => rolesIniciales.Contains(r.Nombre))
                .Select(r => r.Nombre)
                .ToList();

            var rolesFaltantes = rolesIniciales
                .Where(nombre => !rolesExistentes.Contains(nombre))
                .Select(nombre => new Rol { Nombre = nombre })
                .ToList();

            if (rolesFaltantes.Count > 0) {
                tiendaContext.Roles.AddRange(rolesFaltantes);
                await tiendaContext.SaveChangesAsync();
            }

            //Esto es para obtener la ruta del proyecto
            var ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!tiendaContext.Marcas.Any()) {
                //leer csv
                using var reader = new StreamReader(ruta + @"/Data/Csvs/Marcas.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<Marca>();
                tiendaContext.Marcas.AddRange(records);
                await tiendaContext.SaveChangesAsync();
            }

            if (!tiendaContext.Categorias.Any()) {
                using var reader = new StreamReader(ruta + @"/Data/Csvs/Categorias.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<Categoria>();
                tiendaContext.Categorias.AddRange(records);
                await tiendaContext.SaveChangesAsync();
            }

            if (!tiendaContext.Productos.Any()) {
                using var reader = new StreamReader(ruta + @"/Data/Csvs/Productos.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<Producto>();
                //si lo hacemos igual que marca y categoria entonces va a intnetar crear las propeidades marca y categorias y daria error
                List<Producto> productos = new List<Producto>();

                foreach (var producto in productos) {
                    productos.Add(new Producto {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        FechaCreacion = producto.FechaCreacion,
                        MarcaId = producto.MarcaId,
                        CategoriaId = producto.CategoriaId
                    });
                }

                tiendaContext.Productos.AddRange(productos);
                await tiendaContext.SaveChangesAsync();
            }

        }
        catch (Exception ex) {
            var logger = loggerFactory.CreateLogger<TiendaContextSeed>();
            logger.LogError(ex.Message);
        }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;

public class Marca {
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public ICollection<Producto> Productos = new List<Producto>();
}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;

public class Marca {
    int Id { get; set; }
    string Nombre { get; set; }
    ICollection<Producto> Productos = new List<Producto>();
}




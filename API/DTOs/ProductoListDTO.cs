using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace API.DTOs;

public class ProductoListDTO : BaseEntity{
    public string Nombre { get; set; } = null!;
    public decimal Precio { get; set; }
    public DateTime? FechaCreacion { get; set; }
}

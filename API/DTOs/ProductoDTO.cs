using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public class ProductoDTO {
    public string Nombre { get; set; } = null!;
    public decimal Precio { get; set; }
    public DateTime? FechaCreacion { get; set; } 
    public int MarcaId { get; set; }
    public int? CategoriaId { get; set; }
}
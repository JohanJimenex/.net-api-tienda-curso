namespace Core.Entities;

public class Categoria : BaseEntity {
    public string? Nombre { get; set; }
    public ICollection<Producto> Productos = new List<Producto>();
}

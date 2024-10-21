namespace Core.Entities;

public class Categoria {
    int Id { get; set; }
    string Nombre { get; set; }
    ICollection<Producto> Productos = new List<Producto>();
}

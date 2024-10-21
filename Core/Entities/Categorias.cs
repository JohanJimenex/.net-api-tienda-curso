namespace Core.Entities;

public class Categorias {
    int Id { get; set; }
    string Nombre { get; set; }
    ICollection<Producto> Productos = new List<Producto>();
}

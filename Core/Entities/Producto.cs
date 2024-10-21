namespace Core.Entities;

public class Producto {

    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public DateTime FechaCreacion { get; set; }

    public int marcaId { get; set; }
    public Marca marca { get; set; }
    
    public Categoria CategoriaId { get; set; }
    public Categoria Categoria { get; set; }




}

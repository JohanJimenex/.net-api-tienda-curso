using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Producto : BaseEntity {


    //este id ya lo tinee la clase base
    // por convencion Entity Framework Core lo toma como llave primaria sabiendo que se llama Id
    // public int Id { get; set; }

    public string? Nombre { get; set; }

    // Colocando reglas con Annotations pero tambien se puede usar Fluent API en el DbContext
    //required pero con mensaje personalizado
    // [Required]
    // [Required(ErrorMessage = "El campo {0} es requerido")]
    // maxLength pero con mensaje personalizado
    // [MaxLength(100)]
    // [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
    // [MinLength(5)]
    // [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Solo se permiten letras y numeros")]
    // [Range(1, 1000)]
    public decimal Precio { get; set; }
    public DateTime FechaCreacion { get; set; }

    //Cuando se coloca el nombre de la propiedad mas ID Entity Framework por convencion sabe que es una llave foranea de la tabla Marca
    public int MarcaId { get; set; }
    public Marca? Marca { get; set; }

    public int? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

}

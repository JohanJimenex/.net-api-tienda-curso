using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;

public class Usuario : BaseEntity {

    public string Nombre { get; set; } = null!;
    public string ApellidoPaterno { get; set; } = null!;
    public string ApellidoMaterno { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Contrasena { get; set; } = null!;
    
    public ICollection<Rol> Roles { get; set; } = new HashSet<Rol>(); // se usa HashSet para evitar duplicados y porque es más rápido que List
    // Esta propiedad se usa para la relación muchos a muchos con la tabla UsuariosRoles por lo que no se agrega la propiedad de navegación
    // public ICollection<UsuariosRoles> UsuariosRoles { get; set; }  = null!; 

}
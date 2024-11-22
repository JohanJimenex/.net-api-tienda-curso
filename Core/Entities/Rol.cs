using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;

public class Rol : BaseEntity {

    public string Nombre { get; set; } = null!;
    public ICollection<Usuario> Usuarios { get; set; } = new HashSet<Usuario>(); // se usa HashSet para evitar duplicados y porque es más rápido
    // public ICollection<UsuariosRoles> UsuariosRoles { get; set; } = new HashSet<UsuariosRoles>();

}

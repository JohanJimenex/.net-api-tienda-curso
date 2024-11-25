using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace API.DTOs;

public class DatosUsuarioDTO {

    public string Nombre { get; set; } = null!;
    public string ApellidoPaterno { get; set; } = null!;
    public string ApellidoMaterno { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public List<string> Roles { get; set; } = null!;
    public string Token { get; set; } = null!;
}

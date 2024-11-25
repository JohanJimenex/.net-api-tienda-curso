using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public class LoginDTO {

    public string Correo { get; set; } = null!;
    public string Contrasena { get; set; } = null!;

}

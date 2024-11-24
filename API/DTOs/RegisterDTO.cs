using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public class RegisterDTO {
    [Required]
    public required string Nombre { get; set; }
    [Required]
    public required string ApellidoPaterno { get; set; }
    [Required]
    public required string ApellidoMaterno { get; set; }
    [Required]
    public required string Correo { get; set; }
    [Required]
    public required string Contrasena { get; set; }
}
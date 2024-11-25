
using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase {

    private readonly IUserService _userService;

    public UsuariosController(IUserService userService) {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO) {
        var result = await _userService.RegisterAsync(registerDTO);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) {

        var result = await _userService.LoginAsync(loginDTO);

        if (result == null) {
            return Unauthorized("Usuario o contraseña incorrectos");
        }

        return Ok(result);
    }

}

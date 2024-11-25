
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class UserService : IUserService {

    private readonly JWT _jwt; //injeccion de dependencia de la clase JWT
    private readonly IUnitOfWork _unitOfWork; //injeccion de dependencia de la interfaz IUnitOfWork
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private IMapper _mapper;
    //Se usa IOptions para inyectar la configuración de JWT ya que la clase JWT es un una clase que se alimenta de un archivo de configuración
    //Se injectaria el servicio asi: Services.AddOptions<JWT>().Bind(Configuration.GetSection("JWT"));
    // esto buscara en el archivo de configuración la sección JWT y la mapeara a la clase JWT
    public UserService(IOptions<JWT> jwt, IUnitOfWork unitOfWork, IPasswordHasher<Usuario> passwordHasher, IMapper mapper) {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<string> RegisterAsync(RegisterDTO registerDTO) {

        //validar si el usuario existe
        var usuarioExiste = _unitOfWork.UsuariosRepository.Find(u => u.Correo.ToLower() == registerDTO.Correo.ToLower()).FirstOrDefault();

        if (usuarioExiste != null) {
            return $"El correo '{registerDTO.Correo}' ya esta registrado";
        }

        var usuario = _mapper.Map<Usuario>(registerDTO);

        usuario.Contrasena = _passwordHasher.HashPassword(usuario, registerDTO.Contrasena); //Se encripta la contraseña
        var rolPredeterminado = _unitOfWork.RolesRepository.Find(r => r.Nombre == Roles.RolPredeterminado.ToString()).First(); //Se busca el rol predeterminado

        try {

            usuario.Roles.Add(rolPredeterminado);
            _unitOfWork.UsuariosRepository.Add(usuario);
            await _unitOfWork.Save();
            return "Usuario registrado exitosamente";
        }
        catch (Exception ex) {
            var message = ex.Message;
            return $"Error al registrar el usuario: {message}";
        }

    }

    public async Task<DatosUsuarioDTO> LoginAsync(LoginDTO loginDTO) {

        var usuario = await _unitOfWork.UsuariosRepository.GetUsuarioByCorreo(loginDTO.Correo.ToLower());

        if (usuario == null) {
            return null!;
        }

        var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Contrasena, loginDTO.Contrasena);

        if (result == PasswordVerificationResult.Failed) {
            return null!;
        }

        // var token = TokenHelper.GenerateToken(usuario, _jwt.SecretKey, _jwt.ExpirationTime);
        var token = GenerateToken(usuario);

        var datosUsuario = _mapper.Map<DatosUsuarioDTO>(usuario);
        datosUsuario.Token = token;

        return datosUsuario;
    }


    public string GenerateToken(Usuario usuario) {
        var key = Encoding.UTF8.GetBytes(_jwt.Key);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var claims = new[]{
            //Pueden haber mas cliams como el nombre, apellido, roles, etc
            new Claim(ClaimTypes.Name, usuario.Nombre), //etc
            new Claim(ClaimTypes.Email, usuario.Correo),
        };

        foreach (var rol in usuario.Roles) {
            claims.Append(new Claim(ClaimTypes.Role, rol.Nombre));
        }

        var tokenDescriptor = new SecurityTokenDescriptor {
            Audience = _jwt.Audience,
            Issuer = _jwt.Issuer,
            Subject = new ClaimsIdentity(claims),
            //Otra forma de agregar claims
            // Claims = new Dictionary<string, object> {
            //     { ClaimTypes.Name, usuario.Correo },
            //     { ClaimTypes.Role, usuario.Roles.FirstOrDefault()?.Nombre ?? string.Empty }
            // },
            Expires = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            SigningCredentials = credentials
        };

        //Esta clase es otra forma de generar el token
        // var tokenDescriptor = new JwtSecurityToken(  
        //     audience: _jwt.Audience,
        //     issuer: _jwt.Issuer,
        //     claims: claims,
        //     expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
        //     signingCredentials: credentials
        // );

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
        // Esta es otra forma de generar el token
        // return tokenHandler.WriteToken(tokenDescriptor2);

    }



}

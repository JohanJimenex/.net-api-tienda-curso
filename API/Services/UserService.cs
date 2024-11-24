
using API.DTOs;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

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
            return $"El correo {registerDTO.Correo} ya esta registrado";
        }

        var usuario = _mapper.Map<Usuario>(registerDTO);
        
        usuario.Contrasena = _passwordHasher.HashPassword(usuario, registerDTO.Contrasena); //Se encripta la contraseña
        var rolPredeterminado = _unitOfWork.RolesRepository.Find(r => r.Nombre == Autorizacion.RolPredeterminado.ToString()).First(); //Se busca el rol predeterminado

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
}

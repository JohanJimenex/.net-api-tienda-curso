using API.DTOs;
using Core.Entities;
namespace API.Services;


public interface IUserService {
    Task<string> RegisterAsync(RegisterDTO registerDTO);
}

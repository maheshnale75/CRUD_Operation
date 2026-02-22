using CrudOperation.DTO_s;
using CrudOperation.Entities;


namespace MontCrestTask.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterUser(RegisterUserDTO registerDto, string imagePath);
        Task<string> Login(LoginUserDTO loginDto); // Return JWT Token
        Task Logout(Guid sessionId);
    }
}

using CrudOperation.DTO_s;
using CrudOperation.Entities;

namespace CrudOperation.Interface_s
{
    public interface IUser
    {

        Task<List<UserResponseDTO>> GetAllUsers();
        Task<User> UpdateUser(RegisterUserDTO user, string newImagePath);
        Task DeleteUser(int id);
    }
}

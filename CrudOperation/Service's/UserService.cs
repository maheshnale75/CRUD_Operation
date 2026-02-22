using CrudOperation.DbData;
using CrudOperation.DTO_s;
using CrudOperation.Entities;
using CrudOperation.Interface_s;
using Microsoft.EntityFrameworkCore;

namespace CrudOperation.Service_s
{
    public class UserService : IUser
    {
        private readonly AppData _Dbcontext;

        public UserService(AppData dbcontext)
        {
            _Dbcontext = dbcontext;
        }

        public async Task<List<UserResponseDTO>> GetAllUsers()
        {
            var users = await (
                from u in _Dbcontext.Users
                join r in _Dbcontext.RoleTables
                    on u.RoleId equals r.Id
                    orderby u.CreatedDateTime descending
                select new UserResponseDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    EmailId = u.EmailId,
                    MobileNumber = u.MobileNumber,
                    Country = u.Country,
                    CreatedDateTime = u.CreatedDateTime,
                    ProfileImagePath = u.ProfileImagePath,
                    Role = r.Role
                }
            ).ToListAsync();

            return users;
        }

        public async Task<User> UpdateUser(RegisterUserDTO user, string newImagePath)
        {
            var existingUser = await _Dbcontext.Users
                                               .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
                throw new Exception("User not found");

            existingUser.UserName = user.UserName;
            existingUser.EmailId = user.EmailId;
            existingUser.MobileNumber = user.MobileNumber;
            existingUser.Country = user.Country;
            existingUser.RoleId = user.RoleId;

            if (!string.IsNullOrEmpty(newImagePath))
            {
                if (!string.IsNullOrEmpty(existingUser.ProfileImagePath))
                {
                    var oldImagePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        existingUser.ProfileImagePath.TrimStart('/')
                    );

                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }

                existingUser.ProfileImagePath = newImagePath;
            }

            await _Dbcontext.SaveChangesAsync();

            return existingUser;
        }

        public async Task DeleteUser(int id)
        {
            var user = await _Dbcontext.Users.FindAsync(id);

            if (user == null)
                throw new Exception("User not found");

            if (!string.IsNullOrEmpty(user.ProfileImagePath))
            {
                var imagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    user.ProfileImagePath.TrimStart('/')
                );

                if (File.Exists(imagePath))
                    File.Delete(imagePath);
            }

            _Dbcontext.Users.Remove(user);
            await _Dbcontext.SaveChangesAsync();
        }

    }
}

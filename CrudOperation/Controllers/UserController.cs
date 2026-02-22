using CrudOperation.DTO_s;
using CrudOperation.Interface_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontCrestTask.Interfaces;

namespace CrudOperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _UserService;

        public UserController(IUser userService)
        {
            _UserService = userService;
        }

        
        [HttpGet("GetAllUsers")]
        [Authorize(Policy = "AdminOrHR")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _UserService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateUser([FromForm] RegisterUserDTO user)
        {
            try
            {
                if (user.Id == null)
                {
                    return BadRequest("Id should not null here");
                }
                string imagePath = string.Empty;

                if (user.ProfileImage != null)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProfileImages");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fileName = Guid.NewGuid().ToString() +
                                   Path.GetExtension(user.ProfileImage.FileName);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.ProfileImage.CopyToAsync(stream);
                    }

                    imagePath = "/ProfileImages/" + fileName;
                }

                var result = await _UserService.UpdateUser(user, imagePath);

                return Ok(new { Message = "User updated successfully", User = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _UserService.DeleteUser(id);
                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}

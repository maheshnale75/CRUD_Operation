using CrudOperation.DTO_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontCrestTask.Interfaces;

namespace MontCrestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registeruser")]
        public async Task<IActionResult> Register([FromForm]RegisterUserDTO User)
        {
            try
            {
                if (string.IsNullOrEmpty(User.EmailId) && (string.IsNullOrEmpty(User.Password) && User.RoleId <= 0))
                {
                    return BadRequest("Invalid input data. Ensure all fields are correctly provided.");
                }
                string imagePath = string.Empty;
                if (User.ProfileImage != null)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProfileImages");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fileName = Guid.NewGuid().ToString() +
                                   Path.GetExtension(User.ProfileImage.FileName);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await User.ProfileImage.CopyToAsync(stream);
                    }

                    imagePath = "/ProfileImages/" + fileName;
                }

                var user = _authService.RegisterUser(User, imagePath);
                return Ok(new { Message = "User registered successfully", User = user });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO User)
        {
            if (User.Input == null && User.Password == null)
            {
                return BadRequest("Invalid request data");
            }
            try
            {

                var token = await _authService.Login(User);
                return Ok(new { Token = token });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> Logout([FromBody] Guid sessionId)
        {
            if (sessionId == Guid.Empty)
            {
                return BadRequest("Invalid session ID");
            }

            try
            {
                await _authService.Logout(sessionId);
                return Ok(new { Message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

       

        

       

    }
}

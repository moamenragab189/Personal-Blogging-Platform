using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personal_Blogging_Platform.Data.DTOs;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Service;
using Talkable.Data.DTOs.Personal_Blogging_Platform.Data.DTOs;

namespace Personal_Blogging_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.Regester(userDto);
            return Created();
        }
        [HttpPatch("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto verifyEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.VerifyEmail(verifyEmail);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await _authService.Login(loginDto);
            return Ok(new { AccessToken = token });
        }

    }
}

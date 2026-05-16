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
        public IActionResult VerifyEmail(VerifyEmailDto verifyEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _authService.VerifyEmail(verifyEmail);
            return Ok();
        }

    }
}

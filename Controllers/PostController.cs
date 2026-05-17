using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Personal_Blogging_Platform.Data.DTOs.Post;
using Personal_Blogging_Platform.Service;
using System.Security.Claims;

namespace Personal_Blogging_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("GeneralPolicy")]
    public class PostController : ControllerBase
    {
        PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }
        [HttpPost]
        public async Task<IActionResult> AddPost(PostRequestDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _postService.AddPost(postDto, userId);
            return Created();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Post()
        {
            var posts = await _postService.GetPosts();
            return Ok(posts);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, PostRequestDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _postService.UpdatePost(id, postDto, userId);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _postService.DeletePost(id, userId);
            return NoContent();

        }
    }
}

using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.Post;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;
using Personal_Blogging_Platform.Exceptions;

namespace Personal_Blogging_Platform.Service
{
    public class PostService
    {
       private readonly PostRepository _repo;
       private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;
        public PostService(PostRepository repo, IMapper mapper, ILogger<PostService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }
        internal async Task AddPost(PostDto postDto, int userId)
        {
            var post = _mapper.Map<Post>(postDto);
            post.AuthorId = userId;
            await _repo.AddPostAsync(post);  
            _logger.LogInformation("Post added successfully with by user ID: {UserId}",  userId);
        }

        internal async Task<List<PostDto>> GetPosts()
        {
          var posts= await _repo.GetPostsAsync();
            _logger.LogInformation("Retrieved {Count} posts.", posts.Count);
            return _mapper.Map<List<PostDto>>(posts);
        }

        internal async Task UpdatePost(int id, PostDto postDto, int userId)
        {
            var post = await _repo.GetPostByIdAsync(id);
            if (post == null )
            {
                _logger.LogWarning("Update failed: Post with ID {PostId} not found.", id);
                throw new NotFoundException("Post not found.");
            }
            if (post.AuthorId != userId)
            {
                _logger.LogWarning("Update failed: User with ID {UserId} does not have permission to update post with ID {PostId}.", userId, id);
                throw new UnauthorizedException("You do not have permission to update this post.");
            }
            _mapper.Map(postDto, post);

            post.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdatePostAsync(post);
            _logger.LogInformation("Post with ID {PostId} updated successfully by user ID {UserId}.", id, userId);
        }

        internal async Task DeletePost(int id, int userId)
        {
            var post = await _repo.GetPostByIdAsync(id);
            if (post == null)
            {
                _logger.LogWarning("Delete failed: Post with ID {PostId} not found.", id);
                throw new NotFoundException("Post not found.");
            }
            if (post.AuthorId != userId)
            {
                _logger.LogWarning("Delete failed: User with ID {UserId} does not have permission to delete post with ID {PostId}.", userId, id);
                throw new UnauthorizedException("You do not have permission to delete this post.");
            }
            await _repo.DeletePostAsync(post);
            _logger.LogInformation("Post with ID {PostId} deleted successfully by user ID {UserId}.", id, userId);
        }
    }
}

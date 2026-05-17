using AutoMapper;
using Microsoft.Extensions.Hosting;
using Personal_Blogging_Platform.Data.DTOs.Comment;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;
using Personal_Blogging_Platform.Exceptions;

namespace Personal_Blogging_Platform.Service
{
    public class CommentService
    {
        private readonly CommentRepository _CommentRepo;
        private readonly PostRepository _postRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;
        public CommentService(CommentRepository commentRepo, PostRepository postRepo, IMapper mapper, ILogger<CommentService> logger)
        {
            _CommentRepo = commentRepo;
            _postRepo = postRepo;
            _mapper = mapper;
            _logger = logger;
        }
        internal async Task AddComment(CommentRequestDto commentDto, int userId)
        {
            _logger.LogInformation("Adding comment for post {PostId} by user {UserId}", commentDto.PostId, userId);

            var comment = _mapper.Map<Comment>(commentDto);
            var post = await _postRepo.GetPostByIdAsync(commentDto.PostId);
            if (post == null)
            {
                _logger.LogWarning("Post with ID {PostId} not found when trying to add comment", commentDto.PostId);
                throw new NotFoundException("Post not found");
            }
         comment.AuthorId = userId;
         await _CommentRepo.AddComment(comment);   
        }

        internal async Task DeleteComment(int id, int userId)
        {
            var comment = await _CommentRepo.GetCommentByIdAsync(id);
            if (comment == null)
            {
                _logger.LogWarning("Comment with ID {CommentId} not found.", id);
                throw new NotFoundException("Comment not found.");
            }
            if (comment.AuthorId != userId)
            {
                _logger.LogWarning("User with ID {UserId} does not have permission to delete comment with ID {CommentId}.", userId, id);
                throw new UnauthorizedException("You do not have permission to delete this comment.");
            }
            await _CommentRepo.DeleteComment(comment);
            _logger.LogInformation("Comment with ID {CommentId} deleted successfully by user ID {UserId}.", id, userId);
        }

        internal async Task<List<CommentResponseDto>> GetCommentsByPostId(int postId)
        {
            var comments = await _CommentRepo.GetCommentsByPostIdAsync(postId);
            if (comments == null || comments.Count == 0)
            {
                    _logger.LogInformation("No comments found for post with ID {PostId}.", postId);
            }

            return _mapper.Map<List<CommentResponseDto>>(comments);
        }
    }
}

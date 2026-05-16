using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.Comment;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;

namespace Personal_Blogging_Platform.Service
{
    public class CommentService
    {
        private readonly CommentRepository _CommentRepo;
        private readonly PostRepository _postRepo;
        private readonly IMapper _mapper;
        public CommentService(CommentRepository commentRepo, PostRepository postRepo, IMapper mapper)
        {
            _CommentRepo = commentRepo;
            _postRepo = postRepo;
            _mapper = mapper;
        }
        internal async Task AddComment(CommentDto commentDto, int userId)
        {
         var comment = _mapper.Map<Comment>(commentDto);
            var post = await _postRepo.GetPostByIdAsync(commentDto.PostId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }
         comment.AuthorId = userId;
         await _CommentRepo.AddComment(comment);   
        }

        internal async Task DeleteComment(int id, int userId)
        {
            var comment = await _CommentRepo.GetCommentByIdAsync(id);
            if (comment == null || comment.AuthorId != userId)
            {
                throw new Exception("Comment not found or you do not have permission to delete this comment.");
            }
            await _CommentRepo.DeleteComment(comment);

        }

        internal async Task<List<CommentDto>> GetCommentsByPostId(int postId)
        {
            var comments = await _CommentRepo.GetCommentsByPostIdAsync(postId);
            if (comments == null)
            {
                throw new Exception("No comments found for this post.");
            }
            return _mapper.Map<List<CommentDto>>(comments);
        }
    }
}

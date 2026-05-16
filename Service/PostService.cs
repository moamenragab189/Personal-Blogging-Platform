using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.Post;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;

namespace Personal_Blogging_Platform.Service
{
    public class PostService
    {
       private readonly PostRepository _repo;
       private readonly IMapper _mapper;
        public PostService(PostRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        internal async Task AddPost(PostDto postDto, int userId)
        {
            var post = _mapper.Map<Post>(postDto);
            post.AuthorId = userId;
            await _repo.AddPostAsync(post);  
        }

        internal async Task<List<PostDto>> GetPosts()
        {
          var posts= await _repo.GetPostsAsync();
           
            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}

using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.Category;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;

namespace Personal_Blogging_Platform.Service
{
    public class CategoryService
    {
        private readonly PostRepository _postRepo;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;
        public CategoryService(PostRepository postRepo, ILogger<CategoryService> logger, IMapper mapper)
        {
            _postRepo = postRepo;
            _logger = logger;
            _mapper = mapper;
         }
        internal async Task AddCategory(CategoryRequestDto categorytDto)
        {
            var category = _mapper.Map<Category>(categorytDto);
            await _postRepo.AddCategory(category);
            _logger.LogInformation("Category added successfully: {Title}", category.Title);
        }

        internal async Task<List<CategoryResponseDto>> GetCategories()
        {
                var categories = await _postRepo.GetCategoriesAsync();
                if (categories == null || categories.Count == 0)
                {
                    _logger.LogInformation("No categories found.");
                }
                _logger.LogInformation("Categories retrieved successfully. Count: {Count}", categories.Count);
                return _mapper.Map<List<CategoryResponseDto>>(categories);
            

        }
    }
}

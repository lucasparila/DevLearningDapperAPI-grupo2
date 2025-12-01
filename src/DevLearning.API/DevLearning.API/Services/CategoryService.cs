using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Category;
using DevLearning.API.Repositories.Interfaces;
using DevLearning.API.Services.Interfaces;

namespace DevLearning.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        private string GenerateUrl(string title)
        {
            string newUrl = title.ToLower().Replace(" ", "-");

            return $"www.devlearning.com.br/categoria/{newUrl}";
        }


        public async Task CreateCategoryAsync(CategoryRequestDTO categoryDto)
        {
            if (await _categoryRepository.CategoryTitleExistsAsync(categoryDto.Title))
                throw new ArgumentException("Já existe uma categoria com este título");

            await _categoryRepository.ShiftOrdersAsync(categoryDto.Order);

            var url = GenerateUrl(categoryDto.Title);

            var category = new Category(
                categoryDto.Title,
                url,
                categoryDto.Summary,
                categoryDto.Order,
                categoryDto.Description,
                false
            );

            await _categoryRepository.CreateCategoryAsync(category);
        }

        public async Task<List<CategoryResponseDTO>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido");

            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
                throw new KeyNotFoundException("Categoria não encontrada");

            return new CategoryResponseDTO
            {
                Id = category.Id,
                Title = category.Title,
                Url = category.Url,
                Summary = category.Summary,
                Order = category.Order,
                Description = category.Description,
                Featured = category.Featured
            };
        }

        public async Task UpdateCategoryAsync(Guid id, CategoryUpdateDTO categoryDto)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.");

            var existing = await _categoryRepository.GetCategoryByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Categoria não encontrada.");

            if (!string.IsNullOrWhiteSpace(categoryDto.Title))
            {
                if (!string.Equals(existing.Title, categoryDto.Title, StringComparison.OrdinalIgnoreCase))
                {
                    if (await _categoryRepository.CategoryTitleExistsForOtherIdAsync(categoryDto.Title, id))
                        throw new ArgumentException("Já existe uma categoria com este título.");
                }

                existing.SetTitle(categoryDto.Title);

                var newUrl = GenerateUrl(categoryDto.Title);
                existing.SetUrl(newUrl);
            }

            if (!string.IsNullOrWhiteSpace(categoryDto.Summary))
            {
                existing.SetSummary(categoryDto.Summary);
            }

            if (!string.IsNullOrWhiteSpace(categoryDto.Description))
            {
                existing.SetDescription(categoryDto.Description);
            }

            if (categoryDto.Featured.HasValue)
            {
                existing.SetFeatured(categoryDto.Featured.Value);
            }

            if (categoryDto.Order.HasValue && categoryDto.Order.Value != existing.Order)
            {

                int oldOrder = existing.Order;

                existing.SetOrder(categoryDto.Order.Value);

                await _categoryRepository.ShiftOrdersForUpdateAsync(oldOrder, categoryDto.Order.Value, id);

            }

            await _categoryRepository.UpdateCategoryAsync(existing);
        }
        public async Task DeleteCategoryAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.");

            var existing = await _categoryRepository.GetCategoryByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Categoria não encontrada.");

            if(await _categoryRepository.HasCourseAsync(id))
                throw new ArgumentException("Não é possível deletar uma categoria que possui cursos associados.");

            await _categoryRepository.DeleteCategoryAsync(id);
        }
        public async Task<CategoryWithCoursesDTO> GetCategoryCoursesAsync(Guid categoryId)
        {
            var (categoryTitle, courses) = await _categoryRepository.GetCategoryCoursesAsync(categoryId);

            if (categoryTitle == null)
                return null;

            return new CategoryWithCoursesDTO
            {
                CategoryTitle = categoryTitle,
                Courses = courses
            };
        }
    }
}

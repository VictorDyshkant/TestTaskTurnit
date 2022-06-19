using Turnit.Abstraction.DTO;

namespace Turnit.Abstraction.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

        Task AddProductToCategory(Guid productId, Guid categoryId);

        Task DeleteProductFromCategory(Guid productId, Guid categoryId);
    }
}

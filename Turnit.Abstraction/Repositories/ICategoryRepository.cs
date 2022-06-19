using Turnit.Abstraction.Entities;

namespace Turnit.Abstraction.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();

    Task AddProductAsync(ProductCategory productCategory);

    Task RemoveProductAsync(ProductCategory productCategory);

    Task<Category> GetCategoryAsync(Guid categoryId);

    Task<ProductCategory> GetProductCategoryAsync(Guid categoryId, Guid productId);
}
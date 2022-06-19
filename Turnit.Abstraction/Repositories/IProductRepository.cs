using Turnit.Abstraction.Entities;

namespace Turnit.Abstraction.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId);

    Task<IEnumerable<Product>> GetUnCategorizedProductsAsync();

    Task<IEnumerable<ProductAvailability>> GetProductAvailabilityAsync(IEnumerable<Guid> productIds);

    Task<Product> GetProductAsync(Guid productId);

    Task ModifyProductAvailabilityAsync(IEnumerable<ProductAvailability> productAvailabilities);

    Task<ProductAvailability> GetProductAvailabilityAsync(Guid storeId, Guid productId);
}
using Turnit.Abstraction.DTO;

namespace Turnit.Abstraction.Services;

public interface IProductService
{
    Task<CategoryInformationDto> GetProductsByCategoryIdAsync(Guid categoryId);

    Task<CategoryInformationDto> GetUnCategorizedProductsAsync();

    Task<IEnumerable<CategoryInformationDto>> GetAllProducts();

    Task BookProductsAsync(Guid productId, IEnumerable<BookDto> bookDto);
}
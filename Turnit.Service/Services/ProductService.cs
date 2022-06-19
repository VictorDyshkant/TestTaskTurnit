using Turnit.Abstraction.DTO;
using Turnit.Abstraction.Entities;
using Turnit.Abstraction.Services;
using Turnit.Abstraction.UnitOfWork;
using Turnit.Common;
using Turnit.Service.Exceptions;

namespace Turnit.Service.Services;

public class ProductService : IProductService
{
    private readonly Func<IUnitOfWork> _unitOfWorkFactory;

    public ProductService(Func<IUnitOfWork> unitOfWorkFactory)
    {
        Requires.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<CategoryInformationDto> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            IEnumerable<Product> products = await unitOfWork.ProductRepository.GetProductsByCategoryAsync(categoryId);
            IEnumerable<ProductAvailability> productAvailabilities = await unitOfWork.ProductRepository.GetProductAvailabilityAsync(products.Select(x => x.Id));

            return FormCategoryInformationDto(products, productAvailabilities, categoryId);
        }
    }

    public async Task<CategoryInformationDto> GetUnCategorizedProductsAsync()
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            IEnumerable<Product> products = await unitOfWork.ProductRepository.GetUnCategorizedProductsAsync();
            IEnumerable<ProductAvailability> productAvailabilities = await unitOfWork.ProductRepository.GetProductAvailabilityAsync(products.Select(x => x.Id));

            return FormCategoryInformationDto(products, productAvailabilities, null);
        }
    }

    public async Task<IEnumerable<CategoryInformationDto>> GetAllProducts()
    {
        List<CategoryInformationDto> categoryInformation = new List<CategoryInformationDto>();

        IEnumerable<Category> categories;
        using (var unitOfWork = _unitOfWorkFactory())
        {
            categories = await unitOfWork.CategoryRepository.GetCategoriesAsync();
        }

        foreach (var category in categories)
        {
            CategoryInformationDto categoryInformationDto = await GetProductsByCategoryIdAsync(category.Id);
            categoryInformation.Add(categoryInformationDto);
        }

        CategoryInformationDto unCategoryInformationDto = await GetUnCategorizedProductsAsync();
        categoryInformation.Add(unCategoryInformationDto);

        return categoryInformation;
    }

    public async Task BookProductsAsync(Guid productId, IEnumerable<BookDto> bookDto)
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            List<ProductAvailability> productAvailabilities = new List<ProductAvailability>();

            foreach (var book in bookDto)
            {
                ProductAvailability productAvailability = await unitOfWork.ProductRepository.GetProductAvailabilityAsync(book.StoreId, productId);

                if (productAvailability is null)
                {
                    throw new NotFoundException($"ProductAvailability for storeId [{book.StoreId}] and productId [{productId}] was not found.");
                }

                if (productAvailability.Availability < book.Quantity)
                {
                    throw new InvalidOperationException("Quantity of booked product can not be greater then availability.");
                }

                productAvailability.Availability -= book.Quantity;
                productAvailabilities.Add(productAvailability);
            }

            unitOfWork.BeginTransaction();
            await unitOfWork.ProductRepository.ModifyProductAvailabilityAsync(productAvailabilities);
            await unitOfWork.CommitAsync();
        }
    }

    private CategoryInformationDto FormCategoryInformationDto(IEnumerable<Product> products, IEnumerable<ProductAvailability> productAvailabilities, Guid? categoryId)
    {
        List<ProductDto> productDto = new List<ProductDto>();
        foreach (var product in products)
        {
            productDto.Add(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Availability = productAvailabilities.Where(pa => pa.Product.Id.Equals(product.Id))
                    .Select(pa => new AvailabilityDto
                    {
                        Availability = pa.Availability,
                        StoreId = pa.Store.Id
                    }).ToList()
            });
        }

        return new CategoryInformationDto
        {
            CategoryId = categoryId,
            Products = productDto
        };
    }
}
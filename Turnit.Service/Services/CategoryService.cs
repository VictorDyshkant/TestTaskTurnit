using AutoMapper;
using Turnit.Abstraction.DTO;
using Turnit.Abstraction.Entities;
using Turnit.Abstraction.Services;
using Turnit.Abstraction.UnitOfWork;
using Turnit.Common;
using Turnit.Service.Exceptions;

namespace Turnit.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly Func<IUnitOfWork> _unitOfWorkFactory;
    private readonly IMapper _mapper;

    public CategoryService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper)
    {
        Requires.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
        Requires.NotNull(mapper, nameof(mapper));

        _unitOfWorkFactory = unitOfWorkFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            IEnumerable<Category> categories = await unitOfWork.CategoryRepository.GetCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }

    public async Task AddProductToCategory(Guid productId, Guid categoryId)
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            Product product = await unitOfWork.ProductRepository.GetProductAsync(productId);
            Category category = await unitOfWork.CategoryRepository.GetCategoryAsync(categoryId);
            IEnumerable<Product> products = await unitOfWork.ProductRepository.GetProductsByCategoryAsync(categoryId);

            if (product is null)
            {
                throw new NotFoundException($"Product for productId [{productId}] was not found.");
            }
            if (category is null)
            {
                throw new NotFoundException($"Category for categoryId [{categoryId}] was not found.");
            }
            if (products.Any(x => x.Id.Equals(productId)))
            {
                throw new InvalidOperationException($"Such product [{productId}] already belong to category [{categoryId}].");
            }

            unitOfWork.BeginTransaction();
            await unitOfWork.CategoryRepository.AddProductAsync(new ProductCategory
            {
                Id = Guid.NewGuid(),
                Category = category,
                Product = product
            });
            await unitOfWork.CommitAsync();
        }
    }

    public async Task DeleteProductFromCategory(Guid productId, Guid categoryId)
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            ProductCategory productCategory = await unitOfWork.CategoryRepository.GetProductCategoryAsync(categoryId, productId);

            if (productCategory is null)
            {
                throw new NotFoundException($"ProductCategory for categoryId [{categoryId}] and productId [{productId}] was not found.");
            }

            unitOfWork.BeginTransaction();
            await unitOfWork.CategoryRepository.RemoveProductAsync(productCategory);
            await unitOfWork.CommitAsync();
        }
    }
}
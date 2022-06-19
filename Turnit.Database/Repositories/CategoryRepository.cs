using NHibernate;
using Turnit.Abstraction.Repositories;
using Turnit.Common;
using Turnit.Entities;

namespace Turnit.Database.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ISession _session;

    public CategoryRepository(ISession session)
    {
        Requires.NotNull(session, nameof(session));
        _session = session;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _session.QueryOver<Category>().ListAsync();
    }

    public async Task AddProductAsync(ProductCategory productCategory)
    {
        await _session.SaveAsync(productCategory);
    }

    public async Task RemoveProductAsync(ProductCategory productCategory)
    {
        await _session.DeleteAsync(productCategory);
    }

    public async Task<Category> GetCategoryAsync(Guid categoryId)
    {
        return await _session.GetAsync<Category>(categoryId);
    }

    public async Task<ProductCategory> GetProductCategoryAsync(Guid categoryId, Guid productId)
    {
        IEnumerable<ProductCategory> productCategories = await _session.QueryOver<ProductCategory>()
            .Where(x => x.Category.Id == categoryId && x.Product.Id == productId)
            .ListAsync();

        return productCategories.FirstOrDefault();
    }
}
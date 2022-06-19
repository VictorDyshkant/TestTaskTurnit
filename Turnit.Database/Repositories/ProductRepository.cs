using NHibernate;
using NHibernate.Criterion;
using Turnit.Abstraction.Repositories;
using Turnit.Common;
using Turnit.Entities;

namespace Turnit.Database.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ISession _session;

    public ProductRepository(ISession session)
    {
        Requires.NotNull(session, nameof(session));
        _session = session;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
    {
        return await _session.QueryOver<ProductCategory>()
            .Where(x => x.Category.Id == categoryId)
            .Select(x => x.Product)
            .ListAsync<Product>();
    }

    public async Task<IEnumerable<Product>> GetUnCategorizedProductsAsync()
    {
        IEnumerable<Product> productWithCategory = await _session.QueryOver<ProductCategory>()
            .Select(x => x.Product)
            .ListAsync<Product>();

        return await _session.QueryOver<Product>()
            .Where(product => !product.Id.IsIn(productWithCategory.Select(x=>x.Id).ToArray()))
            .ListAsync();
    }

    public async Task<IEnumerable<ProductAvailability>> GetProductAvailabilityAsync(IEnumerable<Guid> productIds)
    {
        return await _session.QueryOver<ProductAvailability>()
            .Where(x => x.Product.Id.IsIn(productIds.ToArray()))
            .ListAsync();
    }

    public async Task<Product> GetProductAsync(Guid productId)
    {
        return await _session.GetAsync<Product>(productId);
    }

    public async Task ModifyProductAvailabilityAsync(IEnumerable<ProductAvailability> productAvailabilities)
    {
        foreach (var productAvailability in productAvailabilities)
        {
            if (_session.Get<ProductAvailability>(productAvailability.Id) is null)
            {
                await _session.SaveAsync(productAvailability);
            }
            else
            {
                await _session.UpdateAsync(productAvailability);
            }
        }
    }

    public async Task<ProductAvailability> GetProductAvailabilityAsync(Guid storeId, Guid productId)
    {
        IEnumerable<ProductAvailability> productAvailabilities = await _session.QueryOver<ProductAvailability>()
            .Where(x => x.Product.Id == productId && x.Store.Id == storeId)
            .ListAsync();

        return productAvailabilities.FirstOrDefault();
    }
}
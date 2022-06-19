using NHibernate;
using Turnit.Abstraction.Repositories;
using Turnit.Common;
using Turnit.Entities;

namespace Turnit.Database.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly ISession _session;

    public StoreRepository(ISession session)
    {
        Requires.NotNull(session, nameof(session));
        _session = session;
    }

    public async Task<IEnumerable<Store>> GetStoresAsync()
    {
        return await _session.QueryOver<Store>().ListAsync();
    }

    public async Task<Store> GetStoreByIdAsync(Guid storeId)
    {
        return await _session.GetAsync<Store>(storeId);
    }
}
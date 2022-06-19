using Turnit.Abstraction.Entities;

namespace Turnit.Abstraction.Repositories;

public interface IStoreRepository
{
    Task<IEnumerable<Store>> GetStoresAsync();

    Task<Store> GetStoreByIdAsync(Guid storeId);
}
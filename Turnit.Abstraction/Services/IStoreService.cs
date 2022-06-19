using Turnit.Abstraction.DTO;

namespace Turnit.Abstraction.Services;

public interface IStoreService
{
    Task<IEnumerable<StoreDto>> GetStoresAsync();

    Task RestockProductsAsync(Guid storeId, IEnumerable<RestockDto> storeAddDto);
}
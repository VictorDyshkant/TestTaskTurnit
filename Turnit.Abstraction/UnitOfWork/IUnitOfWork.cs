using Turnit.Abstraction.Repositories;

namespace Turnit.Abstraction.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }

    IProductRepository ProductRepository { get; }

    IStoreRepository StoreRepository { get; }

    void BeginTransaction();

    Task CommitAsync();
}
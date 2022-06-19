using NHibernate;
using Turnit.Abstraction.Repositories;
using Turnit.Abstraction.UnitOfWork;
using Turnit.Common;

namespace Turnit.Database.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ISession _session;
    private ITransaction _transaction;
    private bool _isDisposed;

    public UnitOfWork(ISessionFactory sessionFactory,
        Func<ISession, ICategoryRepository> categoryRepositoryFactory,
        Func<ISession, IProductRepository> productRepositoryFactory,
        Func<ISession, IStoreRepository> storeRepositoryFactory)
    {
        Requires.NotNull(sessionFactory, nameof(sessionFactory));

        _session = sessionFactory.OpenSession();

        CategoryRepository = categoryRepositoryFactory(_session);
        ProductRepository = productRepositoryFactory(_session);
        StoreRepository = storeRepositoryFactory(_session);
    }

    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IStoreRepository StoreRepository { get; }

    public void BeginTransaction()
    {
        if (_transaction is not null)
        {
            _transaction.Dispose();
        }

        _transaction = _session.BeginTransaction();
    }

    public async Task CommitAsync()
    {
        if (_transaction is null || !_transaction.IsActive)
        {
            throw new InvalidOperationException("Not possible to commit changes as transaction was not opened or it is inactive.");
        }

        try
        {
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            if (_transaction is not null)
            {
                _transaction.Dispose();
            }

            if (_session is not null)
            {
                _session.Dispose();
            }

            _isDisposed = true;
        }
    }
}
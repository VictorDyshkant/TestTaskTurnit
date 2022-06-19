using AutoMapper;
using Turnit.Abstraction.DTO;
using Turnit.Abstraction.Entities;
using Turnit.Abstraction.Services;
using Turnit.Abstraction.UnitOfWork;
using Turnit.Common;
using Turnit.Service.Exceptions;

namespace Turnit.Service.Services;

public class StoreService : IStoreService
{
    private readonly Func<IUnitOfWork> _unitOfWorkFactory;
    private readonly IMapper _mapper;

    public StoreService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper)
    {
        Requires.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
        Requires.NotNull(mapper, nameof(mapper));

        _unitOfWorkFactory = unitOfWorkFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StoreDto>> GetStoresAsync()
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            IEnumerable<Store> stores = await unitOfWork.StoreRepository.GetStoresAsync();
            return _mapper.Map<IEnumerable<StoreDto>>(stores);
        }
    }

    public async Task RestockProductsAsync(Guid storeId, IEnumerable<RestockDto> restockDto)
    {
        using (var unitOfWork = _unitOfWorkFactory())
        {
            List<ProductAvailability> productAvailabilities = new List<ProductAvailability>(restockDto.Count());
            Store store = await unitOfWork.StoreRepository.GetStoreByIdAsync(storeId); 
            
            if (store is null)
            {
                throw new NotFoundException($"Store with storeId [{storeId}] was not found.");
            }

            foreach (var restock in restockDto)
            {
                ProductAvailability productAvailability = await unitOfWork.ProductRepository.GetProductAvailabilityAsync(storeId, restock.ProductId);

                if (productAvailability is null)
                {
                    throw new NotFoundException($"ProductAvailability for storeId [{storeId}] and productId [{restock.ProductId}] was not found.");
                }
                
                if (productAvailability is null)
                {
                    productAvailability = new ProductAvailability
                    {
                        Id = Guid.NewGuid(),
                        Availability = restock.Quantity,
                        Product = await unitOfWork.ProductRepository.GetProductAsync(restock.ProductId),
                        Store = store
                    };
                }

                productAvailability.Availability = restock.Quantity;
                productAvailabilities.Add(productAvailability);
            }

            unitOfWork.BeginTransaction();
            await unitOfWork.ProductRepository.ModifyProductAvailabilityAsync(productAvailabilities);
            await unitOfWork.CommitAsync();
        }
    }
}
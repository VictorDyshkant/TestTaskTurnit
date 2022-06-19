using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Turnit.Abstraction.DTO;
using Turnit.Abstraction.Services;
using Turnit.Common;
using Turnit.GenericStore.Api.Models;

namespace Turnit.GenericStore.Api.Controllers
{
    [Route("stores")]
    public class StoreController : ApiControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public StoreController(IStoreService storeService, IMapper mapper)
        {
            Requires.NotNull(storeService, nameof(storeService));
            Requires.NotNull(mapper, nameof(mapper));
            
            _storeService = storeService;
            _mapper = mapper;
        }

        [HttpGet, Route("")]
        public async Task<StoreModel[]> AllStores()
        {
            IEnumerable<StoreDto> storeDto = await _storeService.GetStoresAsync();
            return _mapper.Map<StoreModel[]>(storeDto);
        }

        [HttpPost, Route("{storeId:guid}/restock")]
        public async Task RestockProducts(Guid storeId, IEnumerable<RestockModel> restockModels)
        {
            IEnumerable<RestockDto> restockDto = _mapper.Map<IEnumerable<RestockDto>>(restockModels);
            await _storeService.RestockProductsAsync(storeId, restockDto);
        }
    }
}

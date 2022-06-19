using AutoMapper;
using Turnit.Abstraction.DTO;
using Turnit.GenericStore.Api.Models;

namespace Turnit.GenericStore.Api.Mapper;

public class DtoToModelMapperProfile : Profile
{
    public DtoToModelMapperProfile()
    {
        CreateMap<CategoryDto, CategoryModel>();

        CreateMap<AvailabilityDto, AvailabilityModel>();
        CreateMap<ProductDto, ProductModel>();
        CreateMap<CategoryInformationDto, CategoryInformationModel>();

        CreateMap<StoreDto, StoreModel>();
        CreateMap<RestockModel, RestockDto>();
        CreateMap<BookModel, BookDto>();
    }
}
using AutoMapper;
using Turnit.Abstraction.DTO;
using Turnit.Abstraction.Entities;

namespace Turnit.Service.Mapper;

public class EntityToDtoMapperProfile : Profile
{
    public EntityToDtoMapperProfile()
    {
        CreateMap<Category, CategoryDto>();

        CreateMap<Store, StoreDto>();
    }
}
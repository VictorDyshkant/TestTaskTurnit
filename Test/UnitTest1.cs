using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NUnit.Framework;
using Turnit.Abstraction.DTO;
using Turnit.Entities;
using Turnit.GenericStore.Api.Mapper;
using Turnit.GenericStore.Api.Models;
using Turnit.Service.Mapper;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var mappingConfigure = new MapperConfiguration(con =>
            {
                con.AddProfiles(new Profile[] { new EntityToDtoMapperProfile(), new DtoToModelMapperProfile() });
            });

            var mapper = mappingConfigure.CreateMapper();

            CategoryInformationDto category = new CategoryInformationDto
            {
                CategoryId = new Guid(),
                Products = new List<ProductDto>
                {
                    new ProductDto
                    {
                        Id = new Guid(),
                        Name = "Name",
                        Availability = new List<AvailabilityDto>
                        {
                            new AvailabilityDto
                            {
                                StoreId = new Guid(),
                                Availability = 1
                            }
                        }
                    }
                }
            };

            IEnumerable<AvailabilityDto> availability = new List<AvailabilityDto>
            {
                new AvailabilityDto
                {
                    StoreId = new Guid(),
                    Availability = 1
                }
            };

            var t = mapper.Map<AvailabilityModel[]>(availability);
            Assert.NotNull(t);
            Assert.AreEqual(t.Length, 1);

            //AvailabilityModel availabilityModel = mapper.Map<AvailabilityModel>(new AvailabilityDto
            //{
            //    StoreId = new Guid(),
            //    Availability = 1
            //});
            //Assert.NotNull(availabilityModel);

            //ProductModel product = mapper.Map<ProductModel>(new ProductDto
            //{
            //    Id = new Guid(),
            //    Name = "Name",
            //    Availability = new List<AvailabilityDto>
            //    {
            //        new AvailabilityDto
            //        {
            //            StoreId = new Guid(),
            //            Availability = 1
            //        }
            //    }
            //});
            //Assert.NotNull(product);


            //CategoryInformationModel res = mapper.Map<CategoryInformationModel>(category);

            //Assert.NotNull(res);
            //Assert.NotNull(res.Products);
            //Assert.NotNull(res.Products.First().Availability);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Turnit.Abstraction.DTO;
using Turnit.Abstraction.Services;
using Turnit.Common;
using Turnit.GenericStore.Api.Filters;
using Turnit.GenericStore.Api.Models;

namespace Turnit.GenericStore.Api.Controllers;

[Route("products")]
public class ProductsController : ApiControllerBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
    {
        Requires.NotNull(productService, nameof(productService));
        Requires.NotNull(categoryService, nameof(categoryService));
        Requires.NotNull(mapper, nameof(mapper));

        _productService = productService;
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet, Route("by-category/{categoryId:guid}")]
    public async Task<CategoryInformationModel> ProductsByCategory(Guid categoryId)
    {
        CategoryInformationDto categoryInformation = await _productService.GetProductsByCategoryIdAsync(categoryId);
        return _mapper.Map<CategoryInformationModel>(categoryInformation);
    }

    [HttpGet, Route("")]
    public async Task<CategoryInformationModel[]> AllProducts()
    {
        IEnumerable<CategoryInformationDto> categoryInformation = await _productService.GetAllProducts();
        return _mapper.Map<CategoryInformationModel[]>(categoryInformation);
    }

    [HttpPut, Route("{productId:guid}/category/{categoryId:guid}")]
    [ExceptionHandlingFilter]
    public async Task AddProductToCategory(Guid productId, Guid categoryId)
    {
        await _categoryService.AddProductToCategory(productId, categoryId);
    }

    [HttpDelete, Route("{productId:guid}/category/{categoryId:guid}")]
    [ExceptionHandlingFilter]
    public async Task DeleteProductFromCategory(Guid productId, Guid categoryId)
    {
        await _categoryService.DeleteProductFromCategory(productId, categoryId);
    }

    [HttpPost, Route("{productId:guid}/book")]
    [ExceptionHandlingFilter]
    public async Task BookProductInStore(Guid productId, IEnumerable<BookModel> bookModels)
    {
        IEnumerable<BookDto> bookDto = _mapper.Map<IEnumerable<BookDto>>(bookModels);
        await _productService.BookProductsAsync(productId, bookDto);
    }
}
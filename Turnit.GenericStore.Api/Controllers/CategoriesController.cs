using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Turnit.Abstraction.DTO;
using Turnit.Abstraction.Services;
using Turnit.Common;
using Turnit.GenericStore.Api.Models;

namespace Turnit.GenericStore.Api.Controllers;

[Route("categories")]
public class CategoriesController : ApiControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        Requires.NotNull(categoryService, nameof(categoryService));
        Requires.NotNull(mapper, nameof(mapper));

        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet, Route("")]
    public async Task<CategoryModel[]> AllCategories()
    {
        IEnumerable<CategoryDto> categories = await _categoryService.GetCategoriesAsync();
        return _mapper.Map<CategoryModel[]>(categories);
    }
}
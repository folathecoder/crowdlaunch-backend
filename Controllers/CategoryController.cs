using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/categories/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IDefaultService<Category> _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(IDefaultService<Category> categoryService, IMapper mapper) {
        _categoryService = categoryService;
        _mapper = mapper;
    }
        
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IList<Category>))]
    public async Task<IActionResult> Get() {
        try
        {
            var categories = await _categoryService.GetAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

    }
        

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(Category), 200)]
    public async Task<IActionResult> Get(string id)
    {
        var category = await _categoryService.GetAsync(id);

        if (category is null)
        {
            return NotFound(
                new { Message = $"Category with id: {id} does not exist." }
            );
        }

        return Ok(category);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Category), 201)]

    public async Task<IActionResult> Post(CategoryCreateDto newCategory)
    {
        var category = _mapper.Map<Category>(newCategory);

        try
        {
            await _categoryService.CreateAsync(category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return CreatedAtAction(nameof(Get), new { id = category.categoryId }, category);
    }

    [HttpPatch("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, CategoryUpdateDto updatedCategory)
    {
        var category = await _categoryService.GetAsync(id);

        if (category is null)
        {
            return NotFound(
                new { Message = $"Category with id: {id} does not exist." }
            );
        }

        var updatedCat = _mapper.Map(updatedCategory, category);

        await _categoryService.UpdateAsync(id, updatedCat);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var category = await _categoryService.GetAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        await _categoryService.RemoveAsync(id);

        return NoContent();
    }
}
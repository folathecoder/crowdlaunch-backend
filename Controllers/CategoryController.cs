using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/categories/[controller]")]
public class CategoryController : ControllerBase
{
  private readonly CategoryService _categoryService;

  public CategoryController(CategoryService categoryService) =>
      _categoryService = categoryService;

  [HttpGet]
  public async Task<List<Category>> Get() =>
      await _categoryService.GetAsync();

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<Category>> Get(string id, [FromHeader] string userId)
  {
    var category = await _categoryService.GetAsync(id);

    if (category is null)
    {
      return NotFound();
    }

    return category;
  }

  [HttpPost]
  [Authorize]

  public async Task<IActionResult> Post(CategoryCreateDto newCategory)
  {
    var category = new Category
    {
      categoryName = newCategory.categoryName,
      categoryDescription = newCategory.categoryDescription,
    };
    await _categoryService.CreateAsync(category);

    return CreatedAtAction(nameof(Get), new { id = category.categoryId }, category);
  }

  [HttpPatch("{id:length(24)}")]
  [Authorize]
  public async Task<IActionResult> Update(string id, CategoryUpdateDto updatedCategory)
  {
    var category = await _categoryService.GetAsync(id);

    if (category is null)
    {
      return NotFound();
    }

    category.categoryName = updatedCategory.categoryName;
    category.categoryDescription = updatedCategory.categoryDescription;
    category.updatedAt = DateTime.UtcNow;

    await _categoryService.UpdateAsync(id, category);

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
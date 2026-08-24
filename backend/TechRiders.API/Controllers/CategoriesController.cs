using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechRiders.Application.DTOs.Requests.Category;
using TechRiders.Application.DTOs.Responses.Category;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all categories")]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll(CancellationToken ct)
    {
        return Ok(await _categoryService.GetAllCategoriesAsync(ct));
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get category by ID")]
    [ProducesResponseType(typeof(CategoryResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CategoryResponse>> GetById(int id, CancellationToken ct)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id, ct);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpGet("main")]
    [SwaggerOperation(Summary = "Get main categories")]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetMain(CancellationToken ct)
    {
        return Ok(await _categoryService.GetMainCategoriesAsync(ct));
    }

    [HttpGet("subcategories/{fatherId:int}")]
    [SwaggerOperation(Summary = "Get subcategories")]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetSubCategories(int fatherId, CancellationToken ct)
    {
        return Ok(await _categoryService.GetSubCategoriesAsync(fatherId, ct));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create category")]
    [ProducesResponseType(typeof(CategoryResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var category = await _categoryService.CreateCategoryAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update category")]
    [ProducesResponseType(typeof(CategoryResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CategoryResponse>> Update(int id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var category = await _categoryService.UpdateCategoryAsync(id, request, ct);
            if (category == null) return NotFound();
            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete category")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _categoryService.DeleteCategoryAsync(id, ct);
        if (!result) return NotFound();
        return NoContent();
    }
}

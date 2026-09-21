using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Requests.KnowledgeArticle;
using TechRiders.Application.DTOs.Responses.KnowledgeArticle;
using TechRiders.Application.Exceptions;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>
/// Tutoriales/artículos de conocimiento (incluye el histórico migrado desde WordPress).
/// </summary>
[Authorize]
[ApiController]
[Route("api/knowledge-articles")]
[Produces("application/json")]
public class KnowledgeArticlesController : BaseApiController
{
    private readonly IKnowledgeArticleService _service;
    private readonly IKnowledgeContentBlobService _blobService;
    private readonly ILogger<KnowledgeArticlesController> _logger;

    public KnowledgeArticlesController(
        IKnowledgeArticleService service,
        IKnowledgeContentBlobService blobService,
        ILogger<KnowledgeArticlesController> logger)
    {
        _service = service;
        _blobService = blobService;
        _logger = logger;
    }

    /// <summary>
    /// Proxy de assets (imagenes) de un KnowledgeArticle: el storage de blobs no permite acceso publico,
    /// asi que el frontend debe pedir las imagenes a traves de este endpoint autenticado en vez de a Blob Storage directamente.
    /// </summary>
    [HttpGet("assets/{*blobPath}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsset(string blobPath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(blobPath)) return NotFound();

        try
        {
            var asset = await _blobService.DownloadBinaryAsync(blobPath, cancellationToken);
            Response.Headers.CacheControl = "public, max-age=86400";
            return File(asset.Content, asset.ContentType);
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedKnowledgeArticleResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedKnowledgeArticleResponse>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] Guid? skillId = null,
        [FromQuery] string? search = null,
        [FromQuery] string? categoryName = null,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 12 : pageSize;

        var result = await _service.GetPagedAsync(page, pageSize, categoryId, skillId, search, categoryName, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(KnowledgeArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<KnowledgeArticleResponse>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var article = await _service.GetBySlugAsync(slug, cancellationToken);
            if (article is null) return NotFound();
            return Ok(article);
        }
        catch (KnowledgeContentUnavailableException ex)
        {
            _logger.LogError(ex, "Contenido no disponible para el slug {Slug}", slug);
            return StatusCode(StatusCodes.Status502BadGateway, new { Success = false, Message = "El contenido de este artículo no está disponible en este momento." });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(KnowledgeArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KnowledgeArticleResponse>> Update(Guid id, [FromBody] UpdateKnowledgeArticleRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var article = await _service.UpdateAsync(id, request, cancellationToken);
            if (article is null) return NotFound();
            return Ok(article);
        }
        catch (KnowledgeContentUnavailableException ex)
        {
            _logger.LogError(ex, "Contenido no disponible tras actualizar {Id}", id);
            return StatusCode(StatusCodes.Status502BadGateway, new { Success = false, Message = "El contenido de este artículo no está disponible en este momento." });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Upsert idempotente por Slug a partir del manifest.json generado por
    /// scripts/intake/wp-import. Rechaza (sin insertar) los artículos cuyo blob de
    /// contenido no exista todavía en el container "knowledge".
    /// </summary>
    [HttpPost("import")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(KnowledgeArticleImportResultResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<KnowledgeArticleImportResultResponse>> Import([FromBody] KnowledgeArticleImportRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.ImportAsync(request, cancellationToken);
        return Ok(result);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Responses.PublicContent;
using TechRiders.Api.Services;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublicContentController : ControllerBase
{
    private readonly IPublicContentService _publicContentService;

    public PublicContentController(IPublicContentService publicContentService)
    {
        _publicContentService = publicContentService ?? throw new ArgumentNullException(nameof(publicContentService));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublicContentResponse))]
    public ActionResult<PublicContentResponse> GetPublicContent()
    {
        var content = _publicContentService.GetPublicContent();
        return Ok(content);
    }
}

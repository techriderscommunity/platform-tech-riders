using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Requests.CommunityPartner;
using TechRiders.Application.DTOs.Responses.CommunityPartner;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/community-partner-applications")]
public sealed class CommunityPartnerApplicationsController : BaseApiController
{
    private readonly ICommunityPartnerApplicationService applicationService;
    private readonly ILogger<CommunityPartnerApplicationsController> logger;

    public CommunityPartnerApplicationsController(
        ICommunityPartnerApplicationService applicationService,
        ILogger<CommunityPartnerApplicationsController> logger)
    {
        this.applicationService = applicationService;
        this.logger = logger;
    }

    [HttpPost]
    [RequestSizeLimit(32_768)]
    [ProducesResponseType(typeof(CommunityPartnerApplicationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommunityPartnerApplicationResponse>> Create(
        [FromBody] CreateCommunityPartnerApplicationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await applicationService.CreateAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (InvalidOperationException exception)
        {
            logger.LogInformation(exception, "Duplicate community partner application rejected");
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Application already exists",
                Detail = exception.Message,
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid application",
                Detail = exception.Message,
            });
        }
    }
}
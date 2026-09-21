using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.CommunityPartner;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Api.Controllers.Public;

[ApiController]
[Route("api/public/communities")]
public sealed class CommunitiesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunitiesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<CommunityResponse>>> Get(CancellationToken cancellationToken)
    {
        var communities = await _unitOfWork.Communities.FindAsync(_ => true, cancellationToken);
        return Ok(communities
            .Where(community => community.IsActive)
            .OrderBy(community => community.Name)
            .Select(community => new CommunityResponse
            {
                Id = community.Id,
                Name = community.Name,
                Description = community.Description,
                Website = community.Website,
                LogoUrl = community.LogoUrl,
                LinkedIn = community.LinkedIn,
                Instagram = community.Instagram,
                X = community.X,
                YouTube = community.YouTube,
                Github = community.Github,
            })
            .ToArray());
    }
}
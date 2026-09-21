using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/profile-media")]
public sealed class ProfileMediaController : BaseApiController
{
    private const long MaxImageBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp"
    };

    private readonly IProfileMediaService _mediaService;

    public ProfileMediaController(IProfileMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyPhoto(CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        return Download(await _mediaService.DownloadUserPhotoAsync(userId.Value, cancellationToken));
    }

    [HttpPut("me")]
    [Authorize]
    [RequestSizeLimit(MaxImageBytes)]
    public async Task<IActionResult> ReplaceMyPhoto(IFormFile file, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        ValidateImage(file);
        await using var stream = file.OpenReadStream();
        await _mediaService.UploadUserPhotoAsync(userId.Value, stream, file.ContentType, cancellationToken);
        return NoContent();
    }

    [HttpDelete("me")]
    [Authorize]
    public async Task<IActionResult> DeleteMyPhoto(CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        await _mediaService.DeleteUserPhotoAsync(userId.Value, cancellationToken);
        return NoContent();
    }

    [HttpGet("communities/{communityId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCommunityLogo(Guid communityId, CancellationToken cancellationToken)
    {
        return Download(await _mediaService.DownloadCommunityLogoAsync(communityId, cancellationToken));
    }

    [HttpPut("communities/{communityId:guid}")]
    [Authorize(Roles = "Admin,Staff")]
    [RequestSizeLimit(MaxImageBytes)]
    public async Task<IActionResult> ReplaceCommunityLogo(Guid communityId, IFormFile file, CancellationToken cancellationToken)
    {
        ValidateImage(file);
        await using var stream = file.OpenReadStream();
        await _mediaService.UploadCommunityLogoAsync(communityId, stream, file.ContentType, cancellationToken);
        return NoContent();
    }

    [HttpDelete("communities/{communityId:guid}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> DeleteCommunityLogo(Guid communityId, CancellationToken cancellationToken)
    {
        await _mediaService.DeleteCommunityLogoAsync(communityId, cancellationToken);
        return NoContent();
    }

    private static void ValidateImage(IFormFile? file)
    {
        if (file is null || file.Length == 0 || file.Length > MaxImageBytes || !AllowedContentTypes.Contains(file.ContentType))
        {
            throw new InvalidDataException("Solo se permiten imágenes JPEG, PNG o WebP de hasta 5 MB.");
        }
    }

    private static IActionResult Download(ProfileMediaContent? content)
    {
        return content is null
            ? new NotFoundResult()
            : new FileContentResult(content.Content, content.ContentType)
            {
                EnableRangeProcessing = true,
                LastModified = DateTimeOffset.UtcNow,
            };
    }

    private Guid? CurrentUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var userId) ? userId : null;
    }
}

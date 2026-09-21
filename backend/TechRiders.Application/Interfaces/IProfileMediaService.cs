namespace TechRiders.Application.Interfaces;

public sealed record ProfileMediaContent(byte[] Content, string ContentType);

public interface IProfileMediaService
{
    Task UploadUserPhotoAsync(Guid userId, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task DeleteUserPhotoAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ProfileMediaContent?> DownloadUserPhotoAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ProfileMediaContent?> DownloadUserPhotoAsync(Guid userId, string? legacySlug, CancellationToken cancellationToken = default);
    Task UploadCommunityLogoAsync(Guid communityId, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task DeleteCommunityLogoAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<ProfileMediaContent?> DownloadCommunityLogoAsync(Guid communityId, CancellationToken cancellationToken = default);
}
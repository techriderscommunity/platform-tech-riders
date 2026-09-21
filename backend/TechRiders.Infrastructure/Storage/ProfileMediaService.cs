using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using TechRiders.Application.Interfaces;

namespace TechRiders.Infrastructure.Storage;

public sealed class ProfileMediaService : IProfileMediaService
{
    private readonly BlobContainerClient _container;

    public ProfileMediaService(IOptions<KnowledgeStorageOptions> options)
    {
        var settings = options.Value;
        var connectionString = settings.ConnectionString
            ?? Environment.GetEnvironmentVariable("Storage__ConnectionString")
            ?? Environment.GetEnvironmentVariable("Storage:ConnectionString");

        _container = !string.IsNullOrWhiteSpace(connectionString)
            ? new BlobServiceClient(connectionString).GetBlobContainerClient(settings.ProfileContainer)
            : new BlobServiceClient(new Uri(settings.AccountUrl), new DefaultAzureCredential()).GetBlobContainerClient(settings.ProfileContainer);
    }

    public Task UploadUserPhotoAsync(Guid userId, Stream content, string contentType, CancellationToken cancellationToken = default) =>
        UploadAsync($"users/{userId:D}/photo", content, contentType, cancellationToken);

    public Task DeleteUserPhotoAsync(Guid userId, CancellationToken cancellationToken = default) =>
        DeleteAsync($"users/{userId:D}/photo", cancellationToken);

    public Task<ProfileMediaContent?> DownloadUserPhotoAsync(Guid userId, CancellationToken cancellationToken = default) =>
        DownloadAsync($"users/{userId:D}/photo", cancellationToken);

    public async Task<ProfileMediaContent?> DownloadUserPhotoAsync(Guid userId, string? legacySlug, CancellationToken cancellationToken = default)
    {
        var content = await DownloadUserPhotoAsync(userId, cancellationToken);
        if (content is not null || string.IsNullOrWhiteSpace(legacySlug))
        {
            return content;
        }

        var normalizedSlug = Slugify(legacySlug);
        foreach (var extension in new[] { ".webp", ".png", ".jpg", ".jpeg" })
        {
            content = await DownloadAsync($"users/{normalizedSlug}{extension}", cancellationToken);
            if (content is not null)
            {
                return content;
            }
        }

        return null;
    }

    public Task UploadCommunityLogoAsync(Guid communityId, Stream content, string contentType, CancellationToken cancellationToken = default) =>
        UploadAsync($"Comuneras/{communityId:D}/logo", content, contentType, cancellationToken);

    public Task DeleteCommunityLogoAsync(Guid communityId, CancellationToken cancellationToken = default) =>
        DeleteAsync($"Comuneras/{communityId:D}/logo", cancellationToken);

    public Task<ProfileMediaContent?> DownloadCommunityLogoAsync(Guid communityId, CancellationToken cancellationToken = default) =>
        DownloadAsync($"Comuneras/{communityId:D}/logo", cancellationToken);

    private async Task UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken)
    {
        await _container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);
        var blob = _container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
        await blob.UploadAsync(content, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType },
            Conditions = null,
        }, cancellationToken);
    }

    private async Task DeleteAsync(string blobName, CancellationToken cancellationToken)
    {
        await _container.GetBlobClient(blobName).DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }

    private async Task<ProfileMediaContent?> DownloadAsync(string blobName, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _container.GetBlobClient(blobName).DownloadContentAsync(cancellationToken);
            return new ProfileMediaContent(response.Value.Content.ToArray(), response.Value.Details.ContentType ?? "application/octet-stream");
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    private static string Slugify(string value)
    {
        var normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);
        var previousWasSeparator = false;

        foreach (var character in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(character);
            if (category == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
                previousWasSeparator = false;
                continue;
            }

            if (!previousWasSeparator)
            {
                builder.Append('-');
                previousWasSeparator = true;
            }
        }

        return builder.ToString().Trim('-').Normalize(NormalizationForm.FormC);
    }
}
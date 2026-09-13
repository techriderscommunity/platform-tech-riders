using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using TechRiders.Application.Interfaces;

namespace TechRiders.Infrastructure.Storage;

/// <summary>
/// Implementación de lectura del contenido histórico de KnowledgeArticles.
/// Prioriza connection string cuando está configurada (local/dev sin managed identity)
/// y usa DefaultAzureCredential como fallback en Azure con identidad gestionada.
/// </summary>
public sealed class KnowledgeContentBlobService : IKnowledgeContentBlobService
{
    private readonly BlobContainerClient _containerClient;

    public KnowledgeContentBlobService(IOptions<KnowledgeStorageOptions> options)
    {
        var settings = options.Value;
        _containerClient = CreateContainerClient(settings);
    }

    public static BlobContainerClient CreateContainerClient(KnowledgeStorageOptions settings)
    {
        var connectionString = settings.ConnectionString;
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("Storage__ConnectionString");
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("Storage:ConnectionString");
        }

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            var serviceClient = new BlobServiceClient(connectionString);
            return serviceClient.GetBlobContainerClient(settings.KnowledgeContainer);
        }

        if (string.IsNullOrWhiteSpace(settings.AccountUrl))
        {
            throw new InvalidOperationException("Knowledge storage account URL is not configured.");
        }

        var defaultServiceClient = new BlobServiceClient(new Uri(settings.AccountUrl), new DefaultAzureCredential());
        return defaultServiceClient.GetBlobContainerClient(settings.KnowledgeContainer);
    }

    public static IReadOnlyList<string> ResolveCandidatePaths(string blobPath)
    {
        if (string.IsNullOrWhiteSpace(blobPath))
        {
            return Array.Empty<string>();
        }

        blobPath = blobPath.Trim();
        var candidates = new List<string> { blobPath };

        if (blobPath.StartsWith("knowledge/", StringComparison.OrdinalIgnoreCase))
        {
            var legacy = "articles/" + blobPath.Substring("knowledge/".Length);
            if (!string.Equals(blobPath, legacy, StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(legacy);
            }
        }

        if (blobPath.StartsWith("articles/", StringComparison.OrdinalIgnoreCase))
        {
            var modern = "knowledge/" + blobPath.Substring("articles/".Length);
            if (!string.Equals(blobPath, modern, StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(modern);
            }
        }

        // Assets (imagenes) del import legacy de WordPress viven bajo "assets/<slug>/<archivo>",
        // un tercer prefijo distinto del usado para el Markdown ("knowledge/" o "articles/").
        if (blobPath.StartsWith("knowledge/", StringComparison.OrdinalIgnoreCase))
        {
            candidates.Add("assets/" + blobPath.Substring("knowledge/".Length));
        }
        else if (blobPath.StartsWith("articles/", StringComparison.OrdinalIgnoreCase))
        {
            candidates.Add("assets/" + blobPath.Substring("articles/".Length));
        }

        return candidates.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    public async Task<bool> ExistsAsync(string blobPath, CancellationToken cancellationToken = default)
    {
        foreach (var candidate in ResolveCandidatePaths(blobPath))
        {
            var blobClient = _containerClient.GetBlobClient(candidate);
            var response = await blobClient.ExistsAsync(cancellationToken);
            if (response.Value)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<string> ReadContentAsync(string blobPath, CancellationToken cancellationToken = default)
    {
        foreach (var candidate in ResolveCandidatePaths(blobPath))
        {
            var blobClient = _containerClient.GetBlobClient(candidate);
            try
            {
                var download = await blobClient.DownloadContentAsync(cancellationToken);
                return download.Value.Content.ToString();
            }
            catch (RequestFailedException)
            {
                // Intentionally retry next candidate.
            }
        }

        throw new FileNotFoundException($"No se encontró el contenido del blob '{blobPath}'.");
    }

    public async Task<KnowledgeAssetContent> DownloadBinaryAsync(string blobPath, CancellationToken cancellationToken = default)
    {
        foreach (var candidate in ResolveCandidatePaths(blobPath))
        {
            var blobClient = _containerClient.GetBlobClient(candidate);
            try
            {
                var download = await blobClient.DownloadContentAsync(cancellationToken);
                var contentType = download.Value.Details.ContentType ?? "application/octet-stream";
                return new KnowledgeAssetContent(download.Value.Content.ToArray(), contentType);
            }
            catch (RequestFailedException)
            {
                // Intentionally retry next candidate.
            }
        }

        throw new FileNotFoundException($"No se encontró el asset del blob '{blobPath}'.");
    }
}

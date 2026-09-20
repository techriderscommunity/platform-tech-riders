using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechRiders.Api.Contracts.Responses.Events;

namespace TechRiders.Api.Controllers.Public;

/// <summary>
/// Catalogo curado de videos de podcast para la pagina publica de eventos.
/// </summary>
// TODO(gap): contenido real (IDs/URLs de YouTube) sin entidad de BD todavia.
// Modelar como entidad PodcastEpisode + gestion admin en una proxima iteracion.
[ApiController]
[Route("api/public/events/podcast-videos")]
[Produces("application/json")]
public class PodcastVideosController : BaseApiController
{
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get podcast videos",
        Description = "Returns curated public podcast videos for the events page. Use playlist to filter specific collections.",
        OperationId = "GetPodcastVideos"
    )]
    [SwaggerResponse(200, "Podcast videos", typeof(IEnumerable<PodcastVideoResponse>))]
    [ProducesResponseType(typeof(IEnumerable<PodcastVideoResponse>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<PodcastVideoResponse>> GetPodcastVideos(
        [FromQuery] int maxResults = 8,
        [FromQuery] string? playlist = null)
    {
        var safeLimit = Math.Clamp(maxResults, 1, 20);
        var normalizedPlaylist = (playlist ?? string.Empty).Trim().ToLowerInvariant();

        var videos = normalizedPlaylist switch
        {
            "profiles" => new[]
            {
                new PodcastVideoResponse
                {
                    VideoId = "J25VQJ7Wx34",
                    Title = "Perfiles profesionales · Episodio 1",
                    Url = "https://www.youtube.com/watch?v=J25VQJ7Wx34&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/J25VQJ7Wx34?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/J25VQJ7Wx34/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "X4mIfCx6XPU",
                    Title = "Perfiles profesionales · Episodio 2",
                    Url = "https://www.youtube.com/watch?v=X4mIfCx6XPU&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/X4mIfCx6XPU?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/X4mIfCx6XPU/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "vncHQDNPjEw",
                    Title = "Perfiles profesionales · Episodio 3",
                    Url = "https://www.youtube.com/watch?v=vncHQDNPjEw&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/vncHQDNPjEw?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/vncHQDNPjEw/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "A856m8nAx6g",
                    Title = "Perfiles profesionales · Episodio 4",
                    Url = "https://www.youtube.com/watch?v=A856m8nAx6g&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/A856m8nAx6g?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/A856m8nAx6g/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "5zfaHALRmis",
                    Title = "Perfiles profesionales · Episodio 5",
                    Url = "https://www.youtube.com/watch?v=5zfaHALRmis&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/5zfaHALRmis?list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO",
                    ThumbnailUrl = "https://i.ytimg.com/vi/5zfaHALRmis/hqdefault.jpg"
                }
            },
            "success-stories" => new[]
            {
                new PodcastVideoResponse
                {
                    VideoId = "HKgt8H8o-nI",
                    Title = "Historias de éxito · Episodio 1",
                    Url = "https://www.youtube.com/watch?v=HKgt8H8o-nI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/HKgt8H8o-nI?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/HKgt8H8o-nI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "RXRqB_Ul_oI",
                    Title = "Historias de éxito · Episodio 2",
                    Url = "https://www.youtube.com/watch?v=RXRqB_Ul_oI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/RXRqB_Ul_oI?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/RXRqB_Ul_oI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "zlZwB1VlY28",
                    Title = "Historias de éxito · Episodio 3",
                    Url = "https://www.youtube.com/watch?v=zlZwB1VlY28&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/zlZwB1VlY28?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/zlZwB1VlY28/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "TAxnDg0kyRI",
                    Title = "Historias de éxito · Episodio 4",
                    Url = "https://www.youtube.com/watch?v=TAxnDg0kyRI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/TAxnDg0kyRI?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/TAxnDg0kyRI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "NwEhryRqSio",
                    Title = "Historias de éxito · Episodio 5",
                    Url = "https://www.youtube.com/watch?v=NwEhryRqSio&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/NwEhryRqSio?list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo",
                    ThumbnailUrl = "https://i.ytimg.com/vi/NwEhryRqSio/hqdefault.jpg"
                }
            },
            "interviews" => new[]
            {
                new PodcastVideoResponse
                {
                    VideoId = "WQp9pZb8shU",
                    Title = "Entrevistas · IA, Copilot y el futuro del desarrollo",
                    Url = "https://www.youtube.com/watch?v=WQp9pZb8shU&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/WQp9pZb8shU?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/WQp9pZb8shU/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "SvZ50wArtaM",
                    Title = "Entrevistas · Agentes de IA en empresa",
                    Url = "https://www.youtube.com/watch?v=SvZ50wArtaM&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/SvZ50wArtaM?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/SvZ50wArtaM/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "baKNCZUbvL8",
                    Title = "Entrevistas · Estudiantes AcademyVerso",
                    Url = "https://www.youtube.com/watch?v=baKNCZUbvL8&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/baKNCZUbvL8?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/baKNCZUbvL8/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "-biqjBJN_cI",
                    Title = "Entrevistas · IA con imágenes",
                    Url = "https://www.youtube.com/watch?v=-biqjBJN_cI&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/-biqjBJN_cI?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/-biqjBJN_cI/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "Gc2sLw3vcvM",
                    Title = "Entrevistas · Microsoft Student Ambassador",
                    Url = "https://www.youtube.com/watch?v=Gc2sLw3vcvM&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/Gc2sLw3vcvM?list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q",
                    ThumbnailUrl = "https://i.ytimg.com/vi/Gc2sLw3vcvM/hqdefault.jpg"
                }
            },
            _ => new[]
            {
                new PodcastVideoResponse
                {
                    VideoId = "YekC-fVM3Ig",
                    Title = "Comunidad, aprendizaje y cerrar ciclos: Tech Riders Talks | Salero de Ming",
                    Url = "https://www.youtube.com/watch?v=YekC-fVM3Ig",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/YekC-fVM3Ig",
                    ThumbnailUrl = "https://i.ytimg.com/vi/YekC-fVM3Ig/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "NHkw3rh1BO8",
                    Title = "Liderazgo técnico, comunidad y crecimiento profesional | Sergio Hernández",
                    Url = "https://www.youtube.com/watch?v=NHkw3rh1BO8",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/NHkw3rh1BO8",
                    ThumbnailUrl = "https://i.ytimg.com/vi/NHkw3rh1BO8/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "qJUUlvvH3_g",
                    Title = "IA, liderazgo y comunidad: experiencia sin filtros | Javier Pallo",
                    Url = "https://www.youtube.com/watch?v=qJUUlvvH3_g",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/qJUUlvvH3_g",
                    ThumbnailUrl = "https://i.ytimg.com/vi/qJUUlvvH3_g/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "IOi91LjE0m4",
                    Title = "Ciberseguridad real: pentesting, red team y LockShields | Marco Carrasco",
                    Url = "https://www.youtube.com/watch?v=IOi91LjE0m4",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/IOi91LjE0m4",
                    ThumbnailUrl = "https://i.ytimg.com/vi/IOi91LjE0m4/hqdefault.jpg"
                },
                new PodcastVideoResponse
                {
                    VideoId = "o6bGKi8y2eY",
                    Title = "De junior a senior: claves reales para crecer en tecnología | María & Elías",
                    Url = "https://www.youtube.com/watch?v=o6bGKi8y2eY",
                    EmbedUrl = "https://www.youtube-nocookie.com/embed/o6bGKi8y2eY",
                    ThumbnailUrl = "https://i.ytimg.com/vi/o6bGKi8y2eY/hqdefault.jpg"
                },
            }
        };

        return Ok(videos.Take(safeLimit));
    }
}

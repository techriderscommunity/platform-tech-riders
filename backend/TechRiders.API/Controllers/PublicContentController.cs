using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechRiders.Api.Contracts.Responses.PublicContent;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublicContentController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;

    public PublicContentController(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublicContentResponse))]
    public async Task<ActionResult<PublicContentResponse>> GetPublicContent(CancellationToken cancellationToken)
    {
        var activeAmbassadors = await _dbContext.Users
            .CountAsync(u => u.IsActive && u.UserRoles.Any(ur => ur.Role.Name.ToLower() == "ambassador" || ur.Role.Name.ToLower() == "embajador"), cancellationToken);
        var activeEvents = await _dbContext.Events.CountAsync(e => e.IsActive, cancellationToken);
        var upcomingSessions = await _dbContext.Sessions.CountAsync(s => s.IsActive, cancellationToken);
        var centerCount = await _dbContext.Centers.CountAsync(c => c.IsActive, cancellationToken);

        var response = new PublicContentResponse
        {
            Home = new HomeContentResponse
            {
                Stats =
                [
                    new MetricItemResponse { Icon = "users", Value = activeAmbassadors.ToString(), Label = "Ambassadors activos" },
                    new MetricItemResponse { Icon = "calendar", Value = activeEvents.ToString(), Label = "Eventos" },
                    new MetricItemResponse { Icon = "book", Value = upcomingSessions.ToString(), Label = "Sesiones" },
                    new MetricItemResponse { Icon = "map", Value = centerCount.ToString(), Label = "Centros" }
                ],
                ProfilePanelCards =
                [
                    new HomeProfileCardResponse { Title = "Riders", Description = "Ruta de especialización y comunidad", Icon = "sparkles", Cta = "Ver comunidad", Link = "/community", Accent = "purple" },
                    new HomeProfileCardResponse { Title = "Actividades", Description = "Eventos, sesiones y mentorías", Icon = "calendar", Cta = "Explorar agenda", Link = "/events", Accent = "blue" },
                    new HomeProfileCardResponse { Title = "Intranet", Description = "Acceso a perfiles y gestión interna", Icon = "shield", Cta = "Entrar", Link = "/intranet", Accent = "green" }
                ],
                PastEventPhotos =
                [
                    new HomePastEventPhotoResponse { Src = "/assets/images/event-1.jpg", Alt = "Evento TechRiders", Label = "Open Day" },
                    new HomePastEventPhotoResponse { Src = "/assets/images/event-2.jpg", Alt = "Sesión TechRiders", Label = "Tech Talks" }
                ]
            },
            Events = new EventsContentResponse
            {
                ParticipationModes =
                [
                    new ParticipationModeResponse { Title = "Presencial", Detail = "Encuentros y actividades en centros y eventos" },
                    new ParticipationModeResponse { Title = "Online", Detail = "Sesiones y mentorías desde cualquier punto" }
                ],
                GalleryGroups =
                [
                    new GalleryGroupResponse
                    {
                        Title = "Eventos destacados",
                        Subtitle = "Experiencias recientes",
                        Items =
                        [
                            new GalleryItemResponse { Src = "/assets/images/gallery-1.jpg", Alt = "Evento 1" },
                            new GalleryItemResponse { Src = "/assets/images/gallery-2.jpg", Alt = "Evento 2" }
                        ]
                    }
                ],
                TalksFallback =
                [
                    new VideoCarouselItemResponse { Title = "TechRiders Community", Src = "/assets/videos/community.mp4" },
                    new VideoCarouselItemResponse { Title = "Mentoría y aprendizaje", Src = "/assets/videos/mentorship.mp4" }
                ]
            },
            Centers = new CentersContentResponse
            {
                Metrics =
                [
                    new MetricItemResponse { Icon = "map", Value = centerCount.ToString(), Label = "Centros activos" },
                    new MetricItemResponse { Icon = "target", Value = "100%", Label = "Cobertura regional" }
                ],
                Cards =
                [
                    new FeatureCardResponse { Icon = "location", Title = "Acceso local", Description = "Centros cerca de ti para actividades y mentorías", Points = ["Presencia cercana", "Conexión directa", "Apoyo técnico"] },
                    new FeatureCardResponse { Icon = "community", Title = "Comunidad", Description = "Encuentros con colegas y expertos", Points = ["Networking", "Aprendizaje", "Mentorías"] }
                ]
            },
            Companies = new CompaniesContentResponse
            {
                ValueCards =
                [
                    new FeatureCardResponse { Icon = "briefcase", Title = "Talento", Description = "Conectamos perfiles con oportunidades reales.", Points = ["Perfil validado", "Experiencia práctica", "Foco de empleo"] },
                    new FeatureCardResponse { Icon = "rocket", Title = "Impacto", Description = "Acelera la presencia de talento tech en el ecosistema.", Points = ["Proyectos", "Visibilidad", "Crecimiento"] }
                ],
                ProcessCards =
                [
                    new ProgressCardResponse { Title = "Selección", Detail = "Recogida de necesidades y perfil", Progress = 25, Status = "Revisión", CtaLabel = "Más información", CtaLink = "/companies" },
                    new ProgressCardResponse { Title = "Aceleración", Detail = "Encuentros y puesta en marcha", Progress = 60, Status = "En curso", CtaLabel = "Ver proceso", CtaLink = "/companies" }
                ]
            },
            Opportunities = new OpportunitiesContentResponse
            {
                Tracks =
                [
                    new ProgressCardResponse { Title = "Talento junior", Detail = "Apertura de primeras oportunidades", Progress = 35, Status = "Activo" },
                    new ProgressCardResponse { Title = "Ambassadors", Detail = "Participación como embajadores", Progress = 72, Status = "En marcha" }
                ],
                Resources =
                [
                    new ResourceCardResponse { Mode = "guide", Title = "Guías de acceso", Summary = "Recursos para iniciar con la comunidad", Tags = ["tutorial", "guía"], Meta = "Disponible", CtaLabel = "Abrir", CtaLink = "/resources" },
                    new ResourceCardResponse { Mode = "event", Title = "Eventos", Summary = "Calendario activo de actividades", Tags = ["agenda", "comunidad"], Meta = "Próximamente", CtaLabel = "Ver agenda", CtaLink = "/events" }
                ]
            },
            WomanTech = new WomanTechContentResponse
            {
                Metrics =
                [
                    new MetricItemResponse { Icon = "female", Value = activeAmbassadors.ToString(), Label = "Perfil femenino activo" },
                    new MetricItemResponse { Icon = "star", Value = "+20%", Label = "Crecimiento" }
                ],
                Journey =
                [
                    new JourneyStepResponse { Step = "1", Title = "Descubrir", Text = "Explora la comunidad y los recursos de TechRiders." },
                    new JourneyStepResponse { Step = "2", Title = "Formar", Text = "Participa en sesiones, talleres y mentorías." },
                    new JourneyStepResponse { Step = "3", Title = "Impulsar", Text = "Acelera tu crecimiento profesional con la comunidad." }
                ]
            },
            Join = new JoinContentResponse
            {
                Metrics =
                [
                    new MetricItemResponse { Icon = "rocket", Value = "4 pasos", Label = "Proceso de ingreso" },
                    new MetricItemResponse { Icon = "people", Value = activeAmbassadors.ToString(), Label = "Miembros activos" }
                ],
                IntakeOptions =
                [
                    new SelectOptionResponse { Label = "Ambassador", Value = "ambassador" },
                    new SelectOptionResponse { Label = "Junior", Value = "junior" },
                    new SelectOptionResponse { Label = "Empresa", Value = "company" }
                ]
            },
            OrientaTech = new OrientaTechContentResponse
            {
                Metrics =
                [
                    new MetricItemResponse { Icon = "book", Value = upcomingSessions.ToString(), Label = "Sesiones" },
                    new MetricItemResponse { Icon = "sparkles", Value = "1:1", Label = "Mentorías" }
                ],
                CoreFeatures =
                [
                    new FeatureCardResponse { Icon = "school", Title = "Formación", Description = "Recursos y contenidos para aprender y crecer.", Points = ["Guías", "Tutoriales", "Mentorías"] },
                    new FeatureCardResponse { Icon = "network", Title = "Comunidad", Description = "Conexión con perfiles y oportunidades.", Points = ["Redes", "Eventos", "Colaboración"] }
                ],
                ParticipationTracks =
                [
                    new ProgressCardResponse { Title = "Junior", Detail = "Acompañamiento inicial", Progress = 40, Status = "Recomendado" },
                    new ProgressCardResponse { Title = "Ambassador", Detail = "Mentor y referente", Progress = 65, Status = "Disponible" }
                ],
                StudySections =
                [
                    new FeatureCardResponse { Icon = "books", Title = "Ruta de aprendizaje", Description = "Recorridos temáticos por diferentes ramas tech.", Points = ["Fundamentos", "Proyectos", "Especialización"] }
                ]
            },
            About = new AboutContentResponse
            {
                Metrics =
                [
                    new MetricItemResponse { Icon = "people", Value = activeAmbassadors.ToString(), Label = "Comunidad" },
                    new MetricItemResponse { Icon = "wrench", Value = "24/7", Label = "Soporte" }
                ],
                SocialLinks =
                [
                    new SocialLinkResponse { Platform = "linkedin", Href = "https://www.linkedin.com/company/techriders" },
                    new SocialLinkResponse { Platform = "github", Href = "https://github.com/techriders" }
                ],
                TeamZones =
                [
                    new TeamZoneResponse
                    {
                        Key = "staff",
                        Title = "Staff",
                        Description = "Equipo de coordinación y soporte",
                        Members =
                        [
                            new TeamMemberResponse { Name = "Equipo TechRiders", Role = "Staff", Photo = "/assets/images/team/staff.jpg", PhotoAlt = "Staff", Socials = [] }
                        ]
                    }
                ]
            },
            Tutorials = new TutorialsContentResponse
            {
                FeaturedCategories = ["Backend", "Frontend", "Data", "Cloud", "IA"]
            },
            Intranet = new IntranetContentResponse
            {
                AmbassadorStatusOptions =
                [
                    new SelectOptionResponse { Label = "Disponible", Value = "available" },
                    new SelectOptionResponse { Label = "Ocupado", Value = "busy" },
                    new SelectOptionResponse { Label = "En descanso", Value = "offline" }
                ],
                AmbassadorAvailabilityOptions =
                [
                    new SelectOptionResponse { Label = "Disponible", Value = "available" },
                    new SelectOptionResponse { Label = "No disponible", Value = "unavailable" }
                ],
                StaffPeriodOptions =
                [
                    new SelectOptionResponse { Label = "Trimestre", Value = "quarter" },
                    new SelectOptionResponse { Label = "Semestre", Value = "semester" }
                ],
                MemberCategoryOptions = ["Junior", "Ambassador", "Empresa", "Staff"],
                SessionStatusOptions = ["Pendiente", "Activa", "Cerrada"],
                JuniorSkillOptions = ["JavaScript", "TypeScript", "C#", "Python", "IA"],
                JuniorAvailabilityOptions =
                [
                    new SelectOptionResponse { Label = "Disponible", Value = "available" },
                    new SelectOptionResponse { Label = "En formación", Value = "training" }
                ]
            }
        };

        return Ok(response);
    }
}

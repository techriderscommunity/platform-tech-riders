using TechRiders.Api.Contracts.Responses.PublicContent;

namespace TechRiders.Api.Services;

public interface IPublicContentService
{
    PublicContentResponse GetPublicContent();
}

public sealed class PublicContentService : IPublicContentService
{
    public PublicContentResponse GetPublicContent()
    {
        return new PublicContentResponse
        {
            Home = BuildHomeContent(),
            Events = BuildEventsContent(),
            Centers = BuildCentersContent(),
            Companies = BuildCompaniesContent(),
            Opportunities = BuildOpportunitiesContent(),
            WomanTech = BuildWomanTechContent(),
            Join = BuildJoinContent(),
            OrientaTech = BuildOrientaTechContent(),
            About = BuildAboutContent(),
            Tutorials = BuildTutorialsContent(),
            Intranet = BuildIntranetContent(),
        };
    }

    private static HomeContentResponse BuildHomeContent()
    {
        return new HomeContentResponse
        {
            Stats =
            [
                new() { Value = "13", Label = "Años de comunidad", Icon = "📅" },
                new() { Value = "1300+", Label = "Tutoriales publicados", Icon = "📚" },
                new() { Value = "50+", Label = "Centros inscritos #FPTOUR", Icon = "🏫" },
                new() { Value = "1500+", Label = "Alumnos #FPTOUR", Icon = "👥" },
                new() { Value = "80+", Label = "Sesiones #FPTOUR", Icon = "🎤" },
                new() { Value = "67", Label = "Sesiones en Tajamar Tech", Icon = "🎤" },
                new() { Value = "20+", Label = "Colaboraciones realizadas con otras comunidades", Icon = "🫂" },
                new() { Value = "5", Label = "Eventos propios", Icon = "🎉" },
            ],
            ProfilePanelCards =
            [
                new() { Title = "Docentes", Description = "Impulsa el talento tecnológico de tu alumnado. Comparte conocimiento y accede a recursos para el aula.", Icon = "🎓", Cta = "Explorar", Link = "/centers", Accent = "violet" },
                new() { Title = "Estudiantes", Description = "Aprende tecnologías, descubre formación y accede a oportunidades para desarrollar tu futuro.", Icon = "🧑‍💻", Cta = "Explorar", Link = "/orienta-tech", Accent = "cyan" },
                new() { Title = "Profesionales", Description = "Impulsa tu carrera en tecnología, comparte experiencia y amplía tu red de contactos.", Icon = "💼", Cta = "Explorar", Link = "/events", Accent = "blue" },
                new() { Title = "Empresas", Description = "Conecta con el talento, participa en eventos y comparte conocimiento real con la comunidad.", Icon = "🏢", Cta = "Explorar", Link = "/companies", Accent = "amber" },
                new() { Title = "Orientadores", Description = "Accede a recursos y actividades tecnológicas para tu alumnado y descubre iniciativas STEM.", Icon = "🧭", Cta = "Explorar", Link = "/orienta-tech", Accent = "pink" },
                new() { Title = "Starters", Description = "Descubre profesiones, formaciones y tus primeros pasos en el mundo tech. No necesitas experiencia.", Icon = "🚀", Cta = "Explorar", Link = "/tutorials", Accent = "teal" },
                new() { Title = "Women in Tech", Description = "Referentes, ayudas, comunidad y oportunidades para mujeres que quieren crecer en tecnología.", Icon = "♀️", Cta = "Explorar", Link = "/woman-tech", Accent = "fuchsia" },
                new() { Title = "Conócenos", Description = "Descubre quiénes somos, nuestra misión, valores y cómo trabajamos para impulsar el talento tech.", Icon = "👥", Cta = "Explorar", Link = "/about-us", Accent = "sky" },
            ],
            PastEventPhotos =
            [
                new() { Src = "assets/techito_salero_ming.jpg", Alt = "Talk de TechRiders", Label = "Talks" },
                new() { Src = "assets/techito_karmela.jpg", Alt = "Encuentro #FPTour", Label = "#FPTour" },
                new() { Src = "assets/techito_bici_tajamar.jpg", Alt = "Evento en Tajamar Tech", Label = "Tajamar Tech" },
                new() { Src = "assets/techito_piscineo.jpg", Alt = "Evento externo de comunidad", Label = "Eventos externos" },
            ],
        };
    }

    private static EventsContentResponse BuildEventsContent()
    {
        return new EventsContentResponse
        {
            ParticipationModes =
            [
                new() { Title = "Asistir", Detail = "Reserva plaza en próximos encuentros y participa en sesiones prácticas." },
                new() { Title = "Ponente", Detail = "Comparte una charla técnica o una experiencia real en formato comunidad." },
                new() { Title = "Colaborar", Detail = "Activa alianzas entre centros, empresas y perfiles técnicos de Tech Riders." },
            ],
            GalleryGroups =
            [
                new()
                {
                    Title = "Talks",
                    Subtitle = "Charlas y encuentros de la comunidad técnica.",
                    Items =
                    [
                        new() { Src = "assets/techito_salero_ming.jpg", Alt = "Talk en evento TechRiders" },
                        new() { Src = "assets/techito_salero_ming.jpg", Alt = "Comunidad participando en una charla" },
                        new() { Src = "assets/techito_salero_ming.jpg", Alt = "Ponencia técnica en TechRiders" },
                    ],
                },
                new()
                {
                    Title = "#FPTour",
                    Subtitle = "Meetups y sesiones en centros de formación.",
                    Items =
                    [
                        new() { Src = "assets/techito_karmela.jpg", Alt = "Evento #FPTour en aula" },
                        new() { Src = "assets/techito_karmela.jpg", Alt = "Networking durante #FPTour" },
                        new() { Src = "assets/techito_karmela.jpg", Alt = "Participantes de #FPTour" },
                    ],
                },
                new()
                {
                    Title = "Eventos externos",
                    Subtitle = "Conferencias, webinars y colaboraciones con otras comunidades.",
                    Items =
                    [
                        new() { Src = "assets/techito_piscineo.jpg", Alt = "Evento externo de la comunidad TechRiders" },
                        new() { Src = "assets/techito_bici_tajamar.jpg", Alt = "Conferencia y networking de TechRiders" },
                        new() { Src = "assets/techito_piscineo.jpg", Alt = "Participación de TechRiders en evento externo" },
                    ],
                },
            ],
            TalksFallback =
            [
                new() { Title = "Comunidad, aprendizaje y cerrar ciclos: Tech Riders Talks | Salero de Ming", Src = "https://www.youtube-nocookie.com/embed/YekC-fVM3Ig" },
                new() { Title = "Liderazgo técnico, comunidad y crecimiento profesional | Sergio Hernández", Src = "https://www.youtube-nocookie.com/embed/NHkw3rh1BO8" },
                new() { Title = "IA, liderazgo y comunidad: experiencia sin filtros | Javier Pallo", Src = "https://www.youtube-nocookie.com/embed/qJUUlvvH3_g" },
                new() { Title = "Ciberseguridad real: pentesting, red team y LockShields | Marco Carrasco", Src = "https://www.youtube-nocookie.com/embed/IOi91LjE0m4" },
                new() { Title = "De junior a senior: claves reales para crecer en tecnología | María & Elías", Src = "https://www.youtube-nocookie.com/embed/o6bGKi8y2eY" },
            ],
        };
    }

    private static CentersContentResponse BuildCentersContent()
    {
        return new CentersContentResponse
        {
            Metrics =
            [
                new() { Icon = "🏫", Value = "Centros", Label = "Red educativa abierta" },
                new() { Icon = "🗺️", Value = "Multi-zona", Label = "Cobertura territorial" },
                new() { Icon = "🎓", Value = "FP + Grado", Label = "Perfiles formativos" },
            ],
            Cards =
            [
                new() { Icon = "📍", Title = "Dónde estudiar", Description = "Explora centros por zona y descubre rutas formativas orientadas a perfiles tech.", Points = ["Búsqueda por provincia", "Ficha de centro", "Programas destacados"] },
                new() { Icon = "🧑‍🏫", Title = "Sesiones para aulas", Description = "Solicita sesiones para estudiantes con foco en empleabilidad, especialización y realidad sectorial.", Points = ["Formato presencial/online", "Temáticas por nivel", "Coordinación con Staff"] },
                new() { Icon = "🤝", Title = "Colaboración educativa", Description = "Conecta con la comunidad para actividades, talleres y propuestas conjuntas de alto impacto.", Points = ["Diseño de iniciativas", "Calendario coordinado", "Seguimiento y continuidad"] },
            ],
        };
    }

    private static CompaniesContentResponse BuildCompaniesContent()
    {
        return new CompaniesContentResponse
        {
            ValueCards =
            [
                new() { Icon = "🎤", Title = "Propón una sesión", Description = "Comparte casos reales y aprendizajes prácticos desde tu organización.", Points = ["Formato adaptable", "Audiencias concretas", "Coordinación operativa"] },
                new() { Icon = "🧠", Title = "Participa en actividades", Description = "Impulsa workshops, retos y formatos de comunidad con impacto formativo.", Points = ["Co-creación con Tech Riders", "Visibilidad de marca técnica", "Continuidad anual"] },
                new() { Icon = "🚀", Title = "Conecta con talento", Description = "Activa itinerarios para detectar perfiles junior y senior alineados con tu stack.", Points = ["Perfiles filtrados", "Canales de contacto", "Seguimiento de pipeline"] },
            ],
            ProcessCards =
            [
                new() { Title = "Definición de colaboración", Detail = "Identificamos objetivo, formato y audiencia de la iniciativa.", Progress = 100, Status = "Paso 1" },
                new() { Title = "Planificación y calendario", Detail = "Alineamos fechas, recursos y coordinación con la comunidad.", Progress = 100, Status = "Paso 2" },
                new() { Title = "Ejecución y seguimiento", Detail = "Publicamos, ejecutamos y medimos resultados para repetir impacto.", Progress = 100, Status = "Paso 3" },
            ],
        };
    }

    private static OpportunitiesContentResponse BuildOpportunitiesContent()
    {
        return new OpportunitiesContentResponse
        {
            Tracks =
            [
                new() { Title = "Primer empleo tech", Detail = "Rutas para perfiles junior con foco en transición real al mercado.", Progress = 78, Status = "Junior", CtaLabel = "Ver guía", CtaLink = "/orienta-tech" },
                new() { Title = "Upskilling profesional", Detail = "Sesiones y recursos para evolución de perfil técnico y liderazgo.", Progress = 65, Status = "Profesional", CtaLabel = "Explorar recursos", CtaLink = "/tutorials" },
                new() { Title = "Conexión con empresas", Detail = "Canales de colaboración, sesiones y oportunidades compartidas con partners.", Progress = 71, Status = "Empresa", CtaLabel = "Ir a empresas", CtaLink = "/companies" },
            ],
            Resources =
            [
                new() { Mode = "Comunidad", Title = "Banco de conocimiento Tech Riders", Summary = "Tutoriales, charlas y materiales prácticos para aprendizaje continuo.", Tags = ["Tutoriales", "Recursos", "Aprendizaje"], Meta = "Actualización continua", CtaLabel = "Ir a conocimiento", CtaLink = "/tutorials" },
                new() { Mode = "Actividad", Title = "Próximas sesiones y actividades", Summary = "Agenda pública con oportunidades para participar y hacer networking.", Tags = ["Eventos", "Sesiones", "Networking"], Meta = "Calendario abierto", CtaLabel = "Ver calendario", CtaLink = "/calendar" },
            ],
        };
    }

    private static WomanTechContentResponse BuildWomanTechContent()
    {
        return new WomanTechContentResponse
        {
            Metrics =
            [
                new() { Icon = "💜", Value = "Woman Tech", Label = "Línea de comunidad" },
                new() { Icon = "🎙️", Value = "Sesiones", Label = "Referentes y experiencias" },
                new() { Icon = "🤝", Value = "Red", Label = "Acompañamiento y visibilidad" },
            ],
            Journey =
            [
                new() { Step = "01", Title = "Inspiración", Text = "Historias y trayectorias de mujeres en tecnología con contexto real." },
                new() { Step = "02", Title = "Aprendizaje", Text = "Sesiones técnicas y recursos para fortalecer habilidades y confianza." },
                new() { Step = "03", Title = "Conexión", Text = "Vínculo con comunidad, redes profesionales y nuevas oportunidades." },
            ],
        };
    }

    private static JoinContentResponse BuildJoinContent()
    {
        return new JoinContentResponse
        {
            Metrics =
            [
                new() { Value = "13", Label = "Años de comunidad", Icon = "📅" },
                new() { Value = "1300+", Label = "Recursos compartidos", Icon = "📚" },
                new() { Value = "80+", Label = "Sesiones #FPTOUR", Icon = "🎤" },
                new() { Value = "1500+", Label = "Alumnos impactados", Icon = "👥" },
            ],
            IntakeOptions =
            [
                new() { Label = "Quiero unirme como miembro", Value = "member" },
                new() { Label = "Quiero solicitar ser Ambassador", Value = "ambassador" },
                new() { Label = "Quiero solicitar una sesión", Value = "session" },
            ],
        };
    }

    private static OrientaTechContentResponse BuildOrientaTechContent()
    {
        return new OrientaTechContentResponse
        {
            Metrics =
            [
                new() { Icon = "📚", Value = "4", Label = "Programas base" },
                new() { Icon = "💼", Value = "20+", Label = "Empresas conectadas" },
                new() { Icon = "🎯", Value = "1:1", Label = "Mentoring personalizado" },
                new() { Icon = "🚀", Value = "360°", Label = "Orientación de carrera" },
            ],
            CoreFeatures =
            [
                new() { Icon = "📚", Title = "Formaciones regladas", Description = "Programas formales para iniciar o transformar tu carrera tecnológica.", Points = ["Ciclos formativos", "Bootcamps certificados", "Rutas guiadas"] },
                new() { Icon = "💼", Title = "Empleo Tech", Description = "Conexión con empresas reales que contratan talento junior y en transición.", Points = ["Ofertas curadas", "Prácticas", "Networking de hiring"] },
                new() { Icon = "🎯", Title = "Mentoría personalizada", Description = "Acompañamiento por profesionales en activo para acelerar tu evolución.", Points = ["Mentor asignado", "Sesiones periódicas", "Seguimiento de objetivos"] },
                new() { Icon = "🚀", Title = "Orientación estratégica", Description = "Plan de carrera con objetivos accionables y revisión continua.", Points = ["Especialización", "Roadmap", "Revisión trimestral"] },
            ],
            ParticipationTracks =
            [
                new() { Title = "Empresas colaboradoras", Status = "Activa", Progress = 78, Detail = "Red de organizaciones que abren oportunidades reales de empleabilidad.", CtaLabel = "Ver oportunidades", CtaLink = "/join" },
                new() { Title = "Recruiters y RRHH", Status = "Activa", Progress = 74, Detail = "Sesiones de mercado laboral, procesos de selección y feedback estructurado.", CtaLabel = "Participar", CtaLink = "/join" },
                new() { Title = "Recursos y contenidos", Status = "En crecimiento", Progress = 69, Detail = "Videoteca, guías y casos para crecer en soft skills y carrera profesional.", CtaLabel = "Explorar", CtaLink = "/tutorials" },
            ],
            StudySections =
            [
                new() { Icon = "", Title = "FP", Description = "Itinerarios base en desarrollo, sistemas, data y ciberseguridad para iniciar carrera tech.", Points = ["SMR", "ASIR", "DAW", "DAM"] },
                new() { Icon = "", Title = "Másteres FP", Description = "Especialización en áreas con alta demanda y enfoque práctico para empleabilidad.", Points = ["Big Data", "Ciberseguridad", "Cloud", "IA aplicada"] },
                new() { Icon = "", Title = "Certificados", Description = "Rutas cortas para validar competencias y acelerar inserción laboral.", Points = ["Qué son", "Cuándo elegirlos", "FAQ"] },
                new() { Icon = "", Title = "Grados y Másteres", Description = "Opciones universitarias orientadas a perfiles técnicos y de especialización avanzada.", Points = ["Grados base", "Másteres de especialidad", "Comparativa por perfil"] },
                new() { Icon = "", Title = "Certificaciones", Description = "Credenciales por fabricante para reforzar tu perfil profesional.", Points = ["Ruta por proveedor", "Nivel recomendado", "Preparación guiada"] },
            ],
        };
    }

    private static AboutContentResponse BuildAboutContent()
    {
        var socials = new List<SocialLinkResponse>
        {
            new() { Platform = "linkedin", Href = "https://www.linkedin.com" },
            new() { Platform = "github", Href = "https://github.com" },
            new() { Platform = "x", Href = "https://x.com" },
            new() { Platform = "instagram", Href = "https://www.instagram.com" },
            new() { Platform = "youtube", Href = "https://www.youtube.com/@TechRidersMedia" },
        };

        return new AboutContentResponse
        {
            Metrics =
            [
                new() { Icon = "🧭", Value = "4", Label = "Líneas de comunidad" },
                new() { Icon = "👥", Value = "1500+", Label = "Participantes" },
                new() { Icon = "🤝", Value = "20+", Label = "Colaboraciones" },
                new() { Icon = "🎤", Value = "80+", Label = "Sesiones" },
            ],
            SocialLinks = socials,
            TeamZones =
            [
                new()
                {
                    Key = "staff",
                    Title = "Staff",
                    Description = "Personas que lideran y coordinan la comunidad.",
                    Members =
                    [
                        BuildTeamMember("Sergio Hierro", "Founder & Community Lead", "assets/staff/sergio-hierro.png", "Foto de Sergio Hierro", socials),
                        BuildTeamMember("Juan Bou", "Program Coordinator", "assets/staff/Juan Bou.jpg", "Foto de Juan Bou", socials),
                        BuildTeamMember("Diego Zapico", "Learning Initiatives", "assets/staff/diego-zapico.png", "Foto de Diego Zapico", socials),
                        BuildTeamMember("Ana Pereira", "Operations & Community Programs", "assets/staff/ana-pereira.jpg", "Foto de Ana Pereira", socials),
                        BuildTeamMember("Borja Piris", "Engineering Mentor", "assets/staff/borja-piris.jpg", "Foto de Borja Piris", socials),
                    ],
                },
                new()
                {
                    Key = "community-leaders",
                    Title = "Community Leaders",
                    Description = "Personas que ayudan a dar forma y operar iniciativas de Tech Riders.",
                    Members =
                    [
                        BuildTeamMember("Mónica Delgado", "Community Leader", "assets/community-leaders/Mónica Delgado.jpg", "Foto de Mónica Delgado", socials),
                        BuildTeamMember("Rodrigo Liberoff", "Community Leader", "assets/community-leaders/Rodrigo Liberoff.jpg", "Foto de Rodrigo Liberoff", socials),
                    ],
                },
                new()
                {
                    Key = "ambassador",
                    Title = "Ambassador",
                    Description = "Personas que participan activamente en actividades y ayudan a extender comunidad.",
                    Members =
                    [
                        BuildTeamMember("María Reina", "Ambassador · Community Speaker", "assets/ambassadors/María Reina.jpg", "Foto de María Reina", socials),
                        BuildTeamMember("Estefany Duran", "Ambassador · Career Talks", "assets/ambassadors/Estefany Duran.jpg", "Foto de Estefany Duran", socials),
                        BuildTeamMember("Celeste Sánchez", "Ambassador · Learning Sessions", "assets/ambassadors/Celeste Sánchez.jpg", "Foto de Celeste Sánchez", socials),
                    ],
                },
                new()
                {
                    Key = "member",
                    Title = "Member",
                    Description = "Personas que se unen y participan en sesiones, actividades y comunidad.",
                    Members =
                    [
                        BuildTeamMember("Marta Moreno", "Member · Frontend Developer", "assets/member/Marta Moreno.png", "Foto de Marta Moreno", socials),
                        BuildTeamMember("Macarena Mamolar", "Member · Product Design", "assets/member/Macarena Mamolar.jpg", "Foto de Macarena Mamolar", socials),
                        BuildTeamMember("Jorge Rodríguez", "Member · Backend Engineer", "assets/member/Jorge Rodríguez.png", "Foto de Jorge Rodríguez", socials),
                        BuildTeamMember("Diego Pérez", "Member · Data & AI", "assets/member/Diego Pérez.png", "Foto de Diego Pérez", socials),
                    ],
                },
            ],
        };
    }

    private static TutorialsContentResponse BuildTutorialsContent()
    {
        return new TutorialsContentResponse
        {
            FeaturedCategories = ["Azure", ".NET", "C#", "Desarrollo", "Windows Server", "Docker", "Kubernetes", "Full Stack", "Seguridad"],
        };
    }

    private static TeamMemberResponse BuildTeamMember(string name, string role, string photo, string photoAlt, IReadOnlyList<SocialLinkResponse> socials)
    {
        return new TeamMemberResponse
        {
            Name = name,
            Role = role,
            Photo = photo,
            PhotoAlt = photoAlt,
            Socials = socials,
        };
    }

    private static IntranetContentResponse BuildIntranetContent()
    {
        return new IntranetContentResponse
        {
            AmbassadorStatusOptions =
            [
                new() { Label = "Todos", Value = "" },
                new() { Label = "Activos", Value = "activo" },
                new() { Label = "Desactivados", Value = "desactivado" },
                new() { Label = "Pendientes", Value = "pendiente" },
            ],
            AmbassadorAvailabilityOptions =
            [
                new() { Label = "Baja disponibilidad", Value = "1 bloque semanal" },
                new() { Label = "Disponibilidad media", Value = "2 o 3 bloques semanales" },
                new() { Label = "Alta disponibilidad", Value = "4 o más bloques semanales" },
            ],
            StaffPeriodOptions =
            [
                new() { Label = "Este mes", Value = "month" },
                new() { Label = "Este año", Value = "year" },
                new() { Label = "Todo", Value = "all" },
            ],
            MemberCategoryOptions = ["FP Tour", "Eventos", "Mentorias", "Podcast", "Comunidad"],
            SessionStatusOptions = ["Pendiente", "Realizada", "Cancelada"],
            JuniorSkillOptions =
            [
                "JavaScript", "TypeScript", "React", "Angular", "Vue.js",
                "Node.js", "Python", "Java", "C++", "HTML", "CSS",
                "Sass", "Bootstrap", "Tailwind", "Git", "Docker",
                "SQL", "MongoDB", "REST APIs", "GraphQL",
            ],
            JuniorAvailabilityOptions =
            [
                new() { Label = "Inmediata", Value = "Inmediata" },
                new() { Label = "En 1 semana", Value = "En 1 semana" },
                new() { Label = "En 2 semanas", Value = "En 2 semanas" },
                new() { Label = "En 1 mes", Value = "En 1 mes" },
            ],
        };
    }
}

using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>
/// Siembra el catálogo inicial de taxonomía (Requisitos Arquitectura §2), finalidades de consentimiento (§8.2)
/// y bases jurídicas (§8.5) según "decisiones_mvp_tech_riders.md". Idempotente: solo crea lo que falte por código.
/// Administrable después desde Staff/Admin; los valores no se eliminan físicamente, solo se desactivan.
/// </summary>
public static class PreferenceCatalogSeedService
{
    private static readonly (string Code, string Name)[] Technologies =
    [
        ("frontend", "Desarrollo Frontend"),
        ("backend", "Desarrollo Backend"),
        ("fullstack", "Full Stack"),
        ("java", "Java"),
        ("dotnet", ".NET"),
        ("python", "Python"),
        ("javascript", "JavaScript"),
        ("typescript", "TypeScript"),
        ("angular", "Angular"),
        ("react", "React"),
        ("cloud", "Cloud"),
        ("azure", "Azure"),
        ("aws", "AWS"),
        ("devops", "DevOps"),
        ("data", "Data"),
        ("ia", "Inteligencia Artificial"),
        ("machine-learning", "Machine Learning"),
        ("ia-generativa", "IA Generativa"),
        ("ciberseguridad", "Ciberseguridad"),
        ("sistemas", "Sistemas"),
        ("redes", "Redes"),
        ("qa-testing", "QA y Testing"),
        ("arquitectura-software", "Arquitectura de Software"),
        ("power-platform", "Power Platform"),
        ("iot", "IoT"),
        ("blockchain", "Blockchain"),
        ("ux-ui", "UX/UI"),
        ("erp", "ERP"),
        ("crm", "CRM"),
        ("business-central", "Business Central"),
        ("productividad", "Productividad"),
        ("soft-skills", "Soft Skills"),
        ("empleabilidad", "Empleabilidad"),
        ("orientacion-profesional", "Orientación profesional"),
        ("comunidad-tecnologica", "Comunidad tecnológica"),
        ("otros", "Otros"),
    ];

    private static readonly (string Code, string Name)[] ContentTypes =
    [
        ("sesion", "Sesión"),
        ("evento", "Evento"),
        ("formacion", "Formación"),
        ("curso", "Curso"),
        ("workshop", "Workshop"),
        ("tutorial", "Tutorial"),
        ("articulo", "Artículo"),
        ("podcast", "Podcast"),
        ("video", "Vídeo"),
        ("talk", "Talk"),
        ("caso-exito", "Caso de éxito"),
        ("recurso-descargable", "Recurso descargable"),
        ("material-sesion", "Material de sesión"),
        ("mentoria", "Mentoría"),
        ("oferta-empleo", "Oferta de empleo"),
        ("oferta-practicas", "Oferta de prácticas"),
        ("beca", "Beca"),
        ("convocatoria", "Convocatoria"),
        ("call-for-sessions", "Call for Sessions"),
        ("oportunidad", "Oportunidad"),
        ("marketplace", "Marketplace"),
    ];

    private static readonly (string Code, string Name)[] Modalities =
    [
        ("presencial", "Presencial"),
        ("online", "Online"),
        ("hibrida", "Híbrida"),
    ];

    private static readonly (string Code, string Name)[] Locations =
    [
        ("madrid", "Madrid"),
        ("otras-provincias", "Otras provincias"),
        ("remoto", "Remoto"),
        ("internacional", "Internacional"),
    ];

    private static readonly (string Code, string Name)[] Levels =
    [
        ("principiante", "Principiante"),
        ("intermedio", "Intermedio"),
        ("avanzado", "Avanzado"),
    ];

    private static readonly (string Code, string Name)[] Objectives =
    [
        ("aprender", "Aprender"),
        ("iniciarse-en-tecnologia", "Iniciarse en tecnología"),
        ("mejorar-conocimientos-tecnicos", "Mejorar conocimientos técnicos"),
        ("mejorar-habilidades-profesionales", "Mejorar habilidades profesionales"),
        ("buscar-empleo", "Buscar empleo"),
        ("buscar-practicas", "Buscar prácticas"),
        ("cambiar-de-empresa", "Cambiar de empresa"),
        ("mejorar-empleabilidad", "Mejorar la empleabilidad"),
        ("recibir-orientacion", "Recibir orientación"),
        ("impartir-sesiones", "Impartir sesiones"),
        ("crear-contenido", "Crear contenido"),
        ("mentorizar", "Mentorizar"),
        ("recibir-mentoria", "Recibir mentoría"),
        ("participar-en-comunidad", "Participar en comunidad"),
        ("captar-talento", "Captar talento"),
        ("contratar-formacion", "Contratar formación"),
        ("encontrar-colaboradores", "Encontrar colaboradores"),
        ("conectar-con-empresas", "Conectar con empresas"),
        ("conectar-con-centros-educativos", "Conectar con centros educativos"),
    ];

    private static readonly (string Code, string Name)[] Channels =
    [
        ("plataforma", "Notificación en plataforma"),
        ("email-transaccional", "Correo electrónico transaccional"),
    ];

    private static readonly (string Code, string Name)[] Frequencies =
    [
        ("inmediata", "Inmediata"),
        ("semanal", "Semanal"),
        ("mensual", "Mensual"),
        ("solo-esenciales", "Solo comunicaciones esenciales"),
    ];

    private static readonly (string Code, string Name)[] ConsentPurposes =
    [
        ("participacion-comunidad", "Participación en la comunidad"),
        ("comunicaciones-operativas", "Comunicaciones operativas"),
        ("comunicaciones-comunidad", "Comunicaciones de comunidad"),
        ("contacto-sesiones", "Contacto para sesiones"),
        ("perfil-publico", "Perfil público"),
        ("imagen-grabaciones", "Imagen y grabaciones"),
        ("empleo-oportunidades", "Empleo y oportunidades"),
        ("formacion-marketplace", "Formación y marketplace"),
    ];

    private static readonly (string Code, string Name)[] AvailabilityInitiatives =
    [
        ("talks", "Talks"),
        ("podcast", "Podcast"),
        ("sesiones-formativas", "Sesiones formativas"),
        ("mentorias", "Mentorías"),
        ("conocimiento", "Conocimiento"),
        ("eventos", "Eventos"),
        ("fptour", "FPTour"),
        ("otras-iniciativas", "Otras iniciativas"),
    ];

    private static readonly (string Code, string Name)[] LegalBases =
    [
        ("consentimiento", "Consentimiento"),
        ("ejecucion-relacion-servicio", "Ejecución de una relación o servicio solicitado"),
        ("obligacion-legal", "Obligación legal"),
        ("interes-legitimo", "Interés legítimo"),
        ("otra-base-dpo", "Otra base aprobada por el DPO"),
    ];

    public static async Task EnsureDefaultsAsync(TechRidersDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        await EnsureDimensionAsync(dbContext, "technology", "Tecnología o temática", Technologies, cancellationToken);
        await EnsureDimensionAsync(dbContext, "content-type", "Tipo de contenido o actividad", ContentTypes, cancellationToken);
        await EnsureDimensionAsync(dbContext, "modality", "Modalidad", Modalities, cancellationToken);
        await EnsureDimensionAsync(dbContext, "location", "Ubicación", Locations, cancellationToken);
        await EnsureDimensionAsync(dbContext, "level", "Nivel", Levels, cancellationToken);
        await EnsureDimensionAsync(dbContext, "objective", "Objetivo", Objectives, cancellationToken);
        await EnsureDimensionAsync(dbContext, "channel", "Canal", Channels, cancellationToken);
        await EnsureDimensionAsync(dbContext, "frequency", "Frecuencia", Frequencies, cancellationToken);
        await EnsureDimensionAsync(dbContext, "availability", "Disponibilidad para colaborar", AvailabilityInitiatives, cancellationToken);

        var existingPurposes = await dbContext.Set<ConsentPurpose>().Select(p => p.Code).ToListAsync(cancellationToken);
        foreach (var (code, name) in ConsentPurposes.Where(p => !existingPurposes.Contains(p.Code)))
        {
            dbContext.Set<ConsentPurpose>().Add(new ConsentPurpose { Id = Guid.NewGuid(), Code = code, Name = name });
        }

        var existingBases = await dbContext.Set<LegalBasis>().Select(b => b.Code).ToListAsync(cancellationToken);
        foreach (var (code, name) in LegalBases.Where(b => !existingBases.Contains(b.Code)))
        {
            dbContext.Set<LegalBasis>().Add(new LegalBasis { Id = Guid.NewGuid(), Code = code, Name = name });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Catálogo de taxonomía, finalidades y bases jurídicas verificado.");
    }

    private static async Task EnsureDimensionAsync(TechRidersDbContext dbContext, string dimensionCode, string dimensionName, (string Code, string Name)[] values, CancellationToken cancellationToken)
    {
        var dimension = await dbContext.Set<PreferenceDimension>().FirstOrDefaultAsync(d => d.Code == dimensionCode, cancellationToken);
        if (dimension is null)
        {
            dimension = new PreferenceDimension { Id = Guid.NewGuid(), Code = dimensionCode, Name = dimensionName };
            await dbContext.Set<PreferenceDimension>().AddAsync(dimension, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var existingValues = await dbContext.Set<PreferenceDimensionValue>()
            .Where(v => v.DimensionId == dimension.Id)
            .Select(v => v.Code)
            .ToListAsync(cancellationToken);

        foreach (var (code, name) in values.Where(v => !existingValues.Contains(v.Code)))
        {
            dbContext.Set<PreferenceDimensionValue>().Add(new PreferenceDimensionValue
            {
                Id = Guid.NewGuid(),
                DimensionId = dimension.Id,
                Code = code,
                Name = name,
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

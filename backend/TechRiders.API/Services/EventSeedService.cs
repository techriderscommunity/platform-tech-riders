using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Siembra eventos publicos oficiales y eventos en radar necesarios para la agenda.</summary>
public static class EventSeedService
{
    private sealed record EventSeed(
        string Name,
        DateTime Start,
        DateTime End,
        string? Url,
        string Location,
        string Format,
        string Priority,
        string Masters,
        string Observations);

    private static readonly EventSeed[] Defaults =
    [
        new(
            "Copilot Dev Days Madrid",
            DateTime.UtcNow.Date.AddDays(14).AddHours(9),
            DateTime.UtcNow.Date.AddDays(14).AddHours(18),
            "https://githubcopilotdevdaysmadrid.techriders.es/",
            "Madrid",
            "Evento oficial",
            "Alta",
            "Desarrollo + IA",
            "Evento oficial para descubrir nuevas formas de crear software con GitHub Copilot y herramientas de IA."
        ),
        new(
            "EmpleaTech",
            DateTime.UtcNow.Date.AddDays(21).AddHours(9),
            DateTime.UtcNow.Date.AddDays(21).AddHours(18),
            "https://empleatech.es/",
            "Madrid",
            "Evento oficial",
            "Alta",
            "Todos",
            "Encuentro de empleabilidad para conectar talento, empresas y oportunidades del ecosistema tecnologico."
        ),
        new(
            "Bizz Summit",
            new DateTime(2026, 10, 2, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 10, 3, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Voluntario para alumnos",
            "Alta",
            "Power Platform",
            "Pendiente confirmar si debe realizarse antes de que comience el master."
        ),
        new(
            "Presentacion Tech Riders",
            new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 10, 16, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Tajamar",
            "Dentro del total de horas",
            "Alta",
            "Todos",
            "Dos horas por la manana y dos horas por la tarde para atender turno de manana y tarde."
        ),
        new(
            "APIAddictsDays",
            new DateTime(2026, 10, 15, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 10, 15, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Voluntario para alumnos",
            "Alta",
            "Desarrollo + IA",
            "Es de pago."
        ),
        new(
            "TechShow: Big Data & AI World / Ciber / Infraestructura / Desarrollo",
            new DateTime(2026, 11, 4, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 11, 5, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Suma horas y se retrasa fin de master",
            "Alta",
            "Todos",
            "Evento multipista: Big Data & AI World, Ciber, Infraestructura y Desarrollo."
        ),
        new(
            "Netcoreconf",
            new DateTime(2026, 11, 15, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 11, 15, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Voluntario para alumnos",
            "Media",
            "Desarrollo + IA",
            "Fecha provisional: noviembre de 2026."
        ),
        new(
            "Academy Verso / similar",
            new DateTime(2026, 11, 27, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 11, 27, 14, 0, 0, DateTimeKind.Utc),
            null,
            "Tajamar",
            "Suma horas y se retrasa fin de master",
            "Media",
            "Todos",
            "Jornada de manana pendiente de confirmar."
        ),
        new(
            "EmpleaTECH en la Nave",
            new DateTime(2027, 1, 20, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 1, 20, 14, 0, 0, DateTimeKind.Utc),
            "https://empleatech.es/",
            "La Nave",
            "Dentro del total de horas",
            "Alta",
            "Todos",
            "Jornada de manana."
        ),
        new(
            "EmpleaTECH en Tajamar",
            new DateTime(2027, 2, 15, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 2, 15, 14, 0, 0, DateTimeKind.Utc),
            "https://empleatech.es/",
            "Tajamar",
            "Dentro del total de horas",
            "Alta",
            "Todos",
            "Fecha provisional: febrero de 2027. Visita de empresas en clase de cada master."
        ),
        new(
            "T3chFest 2027",
            new DateTime(2027, 3, 15, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 3, 15, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Voluntario para alumnos",
            "Media",
            "Desarrollo + IA",
            "Fecha provisional: marzo de 2027."
        ),
        new(
            "GitHub Copilot Dev Day",
            new DateTime(2027, 4, 9, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 4, 9, 14, 0, 0, DateTimeKind.Utc),
            "https://githubcopilotdevdaysmadrid.techriders.es/",
            "Tajamar",
            "Suma horas y se retrasa fin de master",
            "Alta",
            "Desarrollo + IA",
            "Organizado por Tech Riders. Jornada de manana."
        ),
        new(
            "Tech Riders Camp",
            new DateTime(2027, 5, 28, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 5, 28, 14, 0, 0, DateTimeKind.Utc),
            null,
            "Tajamar",
            "Dentro del total de horas",
            "Alta",
            "Todos",
            "Organizado por Tech Riders. Un par de horas por la manana; ideal tambien por la tarde."
        ),
        new(
            "Codemotion",
            new DateTime(2027, 5, 12, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 5, 13, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Voluntario para alumnos",
            "Media",
            "Desarrollo + IA",
            "Es de pago."
        ),
        new(
            "AWS Summit",
            new DateTime(2027, 6, 15, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 6, 15, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Dentro del total de horas",
            "Alta",
            "Desarrollo + IA + Sistemas + Power Platform",
            "Fecha provisional: junio de 2027."
        ),
        new(
            "Sesiones de etica",
            new DateTime(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 10, 1, 11, 0, 0, DateTimeKind.Utc),
            null,
            "Tajamar",
            "Dentro del total de horas",
            "Alta",
            "Todos",
            "Una hora al mes por la manana y otra por la tarde. Se agruparian todos los masteres de cada turno. Fecha inicial provisional."
        ),
        new(
            "Evento de tres dias de Ciber",
            new DateTime(2027, 3, 1, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 3, 3, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Suma horas y se retrasa fin de master",
            "Alta",
            "Ciber Redes + Hacking + Ciber Cloud",
            "Fecha provisional pendiente de confirmar."
        ),
        new(
            "Data Saturday",
            new DateTime(2026, 11, 21, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 11, 21, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Sabado. Voluntario para alumnos",
            "Alta",
            "Desarrollo + IA",
            "Fecha provisional: sabado de noviembre de 2026."
        ),
        new(
            "Global Agent",
            new DateTime(2027, 2, 20, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 2, 20, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Sabado. Voluntario para alumnos",
            "Alta",
            "Desarrollo + IA",
            "Fecha provisional: febrero de 2027."
        ),
        new(
            "Global Azure",
            new DateTime(2027, 4, 24, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 4, 24, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Sabado. Voluntario para alumnos",
            "Alta",
            "Desarrollo + IA + Sistemas",
            "Fecha provisional: abril de 2027."
        ),
        new(
            "Global Power Platform",
            new DateTime(2027, 5, 22, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2027, 5, 22, 18, 0, 0, DateTimeKind.Utc),
            null,
            "Por confirmar",
            "Sabado. Voluntario para alumnos",
            "Alta",
            "Desarrollo + IA + Sistemas + Power Platform",
            "Fecha provisional: mayo de 2027."
        ),
    ];

    public static async Task EnsureDefaultsAsync(TechRidersDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        var eventType = await dbContext.Set<EventType>()
            .OrderBy(type => type.Name == "Sesión formativa" ? 0 : 1)
            .FirstOrDefaultAsync(cancellationToken);

        if (eventType is null)
        {
            logger.LogWarning("Official event seed skipped because no EventType exists.");
            return;
        }

        var created = 0;
        var updated = 0;

        foreach (var seed in Defaults)
        {
            var existing = await dbContext.Set<Event>()
                .FirstOrDefaultAsync(evento => evento.Name == seed.Name, cancellationToken);

            if (existing is null)
            {
                dbContext.Set<Event>().Add(new Event
                {
                    Id = Guid.NewGuid(),
                    Name = seed.Name,
                    Description = BuildDescription(seed),
                    Url = seed.Url,
                    Location = seed.Location,
                    StartDate = seed.Start,
                    EndDate = seed.End,
                    EventTypeId = eventType.Id,
                    IsActive = true,
                });
                created++;
                continue;
            }

            existing.Description = BuildDescription(seed);
            existing.Url = seed.Url;
            existing.Location = seed.Location;
            existing.StartDate = seed.Start;
            existing.EndDate = seed.End;
            existing.EventTypeId = existing.EventTypeId == Guid.Empty ? eventType.Id : existing.EventTypeId;
            existing.IsActive = true;
            existing.UpdatedAt = DateTime.UtcNow;
            updated++;
        }

        if (created > 0 || updated > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation("Official public events verified ({CreatedCount} created, {UpdatedCount} updated).", created, updated);
    }

    private static string BuildDescription(EventSeed seed) =>
        $"{seed.Observations}\nFormato: {seed.Format}. Prioridad: {seed.Priority}. Masteres objetivo: {seed.Masters}.";
}

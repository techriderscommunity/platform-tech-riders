namespace TechRiders.Domain.Entities.Empleo;

/// <summary>
/// Estado de una candidatura a una oferta
/// </summary>
public enum CandidaturaEstado
{
    Pendiente = 0,
    Entrevista = 1,
    Rechazado = 2,
    Contratado = 3
}

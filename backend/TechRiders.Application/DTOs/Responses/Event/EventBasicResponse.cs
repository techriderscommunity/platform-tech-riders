namespace TechRiders.Application.DTOs.Responses.Event
{
    /// <summary>
    /// DTO de respuesta para información básica de un evento
    /// Usado en relaciones para evitar referencias circulares
    /// </summary>
    public class EventBasicResponse
    {
        /// <summary>
        /// Identificador del evento
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del evento
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de inicio del evento
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de finalización del evento
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}

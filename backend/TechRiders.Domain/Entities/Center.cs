using System.ComponentModel.DataAnnotations.Schema;

namespace TechRiders.Domain.Entities;

public sealed class Center : BaseEntity
{
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Locality { get; set; }
    public string? Location { get; set; }
    public string? ParkingInfo { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }

    // Compatibilidad con campos legacy que siguen usándose por DTOs y repositorios
    public string? ContactPerson { get; set; }
    public string? Specialty { get; set; }
    public string? Parking { get => ParkingInfo; set => ParkingInfo = value; }

    [NotMapped]
    public ICollection<CenterStudy> Studies { get; set; } = new List<CenterStudy>();
    public ICollection<CenterContact> Contacts { get; set; } = new List<CenterContact>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<FPTour> FPTours { get; set; } = new List<FPTour>();
}

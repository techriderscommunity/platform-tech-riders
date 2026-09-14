namespace TechRiders.Domain.Enums;

/// <summary>Nivel de visibilidad de un campo de perfil (Requisitos Arquitectura §10). Por defecto Private.</summary>
public enum FieldVisibility
{
    Private = 1,
    Members = 2,
    Public = 3,
    StaffOnly = 4
}

namespace TechRiders.Domain.Entities;

public sealed class MT_Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? FatherId { get; set; }
    public bool Active { get; set; } = true;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public MT_Category? Main { get; set; }
    public ICollection<MT_Category> Secondary { get; set; } = new List<MT_Category>();
}

public enum Modalidad
{
    Presencial = 0,
    Remota = 1,
    Hibrida = 2
}

public enum OfertaEstado
{
    Borrador = 0,
    Activa = 1,
    Cerrada = 2
}

public sealed class Oferta
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Titulo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
    public Modalidad Modalidad { get; set; } = Modalidad.Hibrida;
    public string Requisitos { get; set; } = string.Empty;
    public OfertaEstado Estado { get; set; } = OfertaEstado.Borrador;
    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public static Oferta Create(
        string titulo,
        string empresa,
        string descripcion,
        decimal salario,
        string ubicacion,
        Modalidad modalidad,
        string requisitos,
        OfertaEstado estado = OfertaEstado.Borrador)
    {
        return new Oferta
        {
            Titulo = titulo,
            Empresa = empresa,
            Descripcion = descripcion,
            Salario = salario,
            Ubicacion = ubicacion,
            Modalidad = modalidad,
            Requisitos = requisitos,
            Estado = estado,
            FechaPublicacion = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void Publicar()
    {
        Estado = OfertaEstado.Activa;
        FechaPublicacion = DateTime.UtcNow;
    }

    public void Cerrar()
    {
        Estado = OfertaEstado.Cerrada;
    }
}

public enum CandidaturaEstado
{
    Pendiente = 0,
    Entrevista = 1,
    Rechazada = 2,
    Contratada = 3
}

public sealed class Candidatura
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OfertaId { get; set; }
    public string JuniorId { get; set; } = string.Empty;
    public string NombreJunior { get; set; } = string.Empty;
    public string EmailJunior { get; set; } = string.Empty;
    public CandidaturaEstado Estado { get; set; } = CandidaturaEstado.Pendiente;
    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public static Candidatura Create(Guid ofertaId, string juniorId, string nombreJunior, string emailJunior)
    {
        return new Candidatura
        {
            OfertaId = ofertaId,
            JuniorId = juniorId,
            NombreJunior = nombreJunior,
            EmailJunior = emailJunior,
            Estado = CandidaturaEstado.Pendiente,
            FechaSolicitud = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void AvanzarAEntrevista()
    {
        Estado = CandidaturaEstado.Entrevista;
    }

    public void Rechazar()
    {
        Estado = CandidaturaEstado.Rechazada;
    }

    public void Contratar()
    {
        Estado = CandidaturaEstado.Contratada;
    }
}

public sealed class Tutorial
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Slug { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Extracto { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string CategoriasJson { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public static Tutorial Create(
        string slug,
        string titulo,
        string extracto,
        string autor,
        string categoriasJson,
        string url)
    {
        return new Tutorial
        {
            Slug = slug,
            Titulo = titulo,
            Extracto = extracto,
            Autor = autor,
            CategoriasJson = categoriasJson,
            Url = url,
            FechaPublicacion = DateTime.UtcNow,
            IsActive = true
        };
    }
}

public sealed class TutorialesPageResult
{
    public TutorialesPageResult(IReadOnlyCollection<Tutorial> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    public IReadOnlyCollection<Tutorial> Items { get; }
    public int TotalCount { get; }
}

public sealed class IntranetAuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public Guid ActorUserId { get; set; }
    public string Result { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}

public sealed class IntranetSetting
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Key { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void Update(string module, string value, string status, string? updatedBy)
    {
        Module = module;
        Value = value;
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }
}

public sealed class IntranetUserCategory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public DateTime? UpdatedAt { get; set; }
}

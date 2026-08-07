using TechRiders.Application.DTOs.Requests;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities.Empleo;
using TechRiders.Domain.Interfaces;
using Mapster;
using MapsterMapper;
namespace TechRiders.Application.Services;

/// <summary>
/// Service for managing job offers and applications
/// </summary>
public class EmpleoService : IEmpleoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EmpleoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    // Oferta operations

    public async Task<IEnumerable<OfertaResponse>> GetAllOfertasAsync(CancellationToken cancellationToken = default)
    {
        var ofertas = await _unitOfWork.Ofertas.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OfertaResponse>>(ofertas);
    }

    public async Task<OfertaResponse?> GetOfertaByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var oferta = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<OfertaResponse?>(oferta);
    }

    public async Task<OfertaResponse> CreateOfertaAsync(CreateOfertaRequest request, CancellationToken cancellationToken = default)
    {
        var oferta = Oferta.Create(
            titulo: request.Titulo,
            empresa: request.Empresa,
            descripcion: request.Descripcion,
            salario: request.Salario,
            ubicacion: request.Ubicacion,
            modalidad: (Modalidad)request.Modalidad,
            requisitos: request.Requisitos
        );

        await _unitOfWork.Ofertas.AddAsync(oferta, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(oferta);
    }

    public async Task<OfertaResponse> UpdateOfertaAsync(UpdateOfertaRequest request, CancellationToken cancellationToken = default)
    {
        var oferta = await _unitOfWork.Ofertas.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {request.Id} not found");

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Titulo)) oferta.Titulo = request.Titulo;
        if (!string.IsNullOrEmpty(request.Empresa)) oferta.Empresa = request.Empresa;
        if (!string.IsNullOrEmpty(request.Descripcion)) oferta.Descripcion = request.Descripcion;
        if (request.Salario.HasValue) oferta.Salario = request.Salario.Value.ToString("F2");
        if (!string.IsNullOrEmpty(request.Ubicacion)) oferta.Ubicacion = request.Ubicacion;
        if (request.Modalidad.HasValue) oferta.Modalidad = (Modalidad)request.Modalidad.Value;
        if (!string.IsNullOrEmpty(request.Requisitos)) oferta.Requisitos = request.Requisitos;
        if (request.Estado.HasValue) oferta.Estado = (OfertaEstado)request.Estado.Value;

        await _unitOfWork.Ofertas.UpdateAsync(oferta, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(oferta);
    }

    public async Task<OfertaResponse> PublishOfertaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var oferta = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {id} not found");

        oferta.Publicar();

        await _unitOfWork.Ofertas.UpdateAsync(oferta, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(oferta);
    }

    public async Task<OfertaResponse> CloseOfertaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var oferta = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {id} not found");

        oferta.Cerrar();

        await _unitOfWork.Ofertas.UpdateAsync(oferta, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(oferta);
    }

    public async Task DeleteOfertaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var oferta = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {id} not found");

        await _unitOfWork.Ofertas.DeleteAsync(oferta, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    // Candidatura operations

    public async Task<IEnumerable<CandidaturaResponse>> GetCandidaturasByOfertaAsync(Guid ofertaId, CancellationToken cancellationToken = default)
    {
        var candidaturas = await _unitOfWork.Candidaturas.GetByOfertaIdAsync(ofertaId, cancellationToken);
        return _mapper.Map<IEnumerable<CandidaturaResponse>>(candidaturas);
    }

    public async Task<IEnumerable<CandidaturaResponse>> GetCandidaturasByJuniorAsync(string juniorId, CancellationToken cancellationToken = default)
    {
        var candidaturas = await _unitOfWork.Candidaturas.GetByJuniorIdAsync(juniorId, cancellationToken);
        return _mapper.Map<IEnumerable<CandidaturaResponse>>(candidaturas);
    }

    public async Task<CandidaturaResponse?> GetCandidaturaByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CandidaturaResponse?>(candidatura);
    }

    public async Task<CandidaturaResponse> CreateCandidaturaAsync(CreateCandidaturaRequest request, CancellationToken cancellationToken = default)
    {
        // Verify offer exists
        var oferta = await _unitOfWork.Ofertas.GetByIdAsync(request.OfertaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {request.OfertaId} not found");

        // Check for duplicate application
        var exists = await _unitOfWork.Candidaturas.ExisteCandidaturaAsync(request.OfertaId, request.JuniorId, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"Application already exists for junior {request.JuniorId} on offer {request.OfertaId}");

        var candidatura = Candidatura.Create(
            ofertaId: request.OfertaId,
            juniorId: request.JuniorId,
            nombreJunior: request.NombreJunior,
            emailJunior: request.EmailJunior
        );

        await _unitOfWork.Candidaturas.AddAsync(candidatura, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(candidatura);
    }

    public async Task<CandidaturaResponse> AdvanceToInterviewAsync(Guid candidaturaId, CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas.GetByIdAsync(candidaturaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {candidaturaId} not found");

        candidatura.AvanzarAEntrevista();

        await _unitOfWork.Candidaturas.UpdateAsync(candidatura, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(candidatura);
    }

    public async Task<CandidaturaResponse> RejectCandidaturaAsync(Guid candidaturaId, CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas.GetByIdAsync(candidaturaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {candidaturaId} not found");

        candidatura.Rechazar();

        await _unitOfWork.Candidaturas.UpdateAsync(candidatura, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(candidatura);
    }

    public async Task<CandidaturaResponse> HireCandidaturaAsync(Guid candidaturaId, CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas.GetByIdAsync(candidaturaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {candidaturaId} not found");

        candidatura.Contratar();

        await _unitOfWork.Candidaturas.UpdateAsync(candidatura, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(candidatura);
    }

    public async Task DeleteCandidaturaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {id} not found");

        await _unitOfWork.Candidaturas.DeleteAsync(candidatura, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

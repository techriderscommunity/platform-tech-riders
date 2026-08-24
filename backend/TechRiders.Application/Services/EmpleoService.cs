using MapsterMapper;
using TechRiders.Application.DTOs.Requests;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

/// <summary>
/// Provides the application workflow for job offers and candidacies.
/// </summary>
public class EmploymentService : IEmploymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmploymentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<OfertaResponse>> GetAllOfertasAsync(CancellationToken cancellationToken = default)
    {
        var offers = await _unitOfWork.Ofertas.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OfertaResponse>>(offers);
    }

    public async Task<OfertaResponse?> GetOfertaByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offer = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<OfertaResponse?>(offer);
    }

    public async Task<OfertaResponse> CreateOfertaAsync(CreateOfertaRequest request, CancellationToken cancellationToken = default)
    {
        var offer = Oferta.Create(
            titulo: request.Titulo,
            empresa: request.Empresa,
            descripcion: request.Descripcion,
            salario: request.Salario,
            ubicacion: request.Ubicacion,
            modalidad: (Modalidad)request.Modalidad,
            requisitos: request.Requisitos
        );

        await _unitOfWork.Ofertas.AddAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(offer);
    }

    public async Task<OfertaResponse> UpdateOfertaAsync(UpdateOfertaRequest request, CancellationToken cancellationToken = default)
    {
        var offer = await _unitOfWork.Ofertas.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {request.Id} not found");

        if (!string.IsNullOrEmpty(request.Titulo)) offer.Titulo = request.Titulo;
        if (!string.IsNullOrEmpty(request.Empresa)) offer.Empresa = request.Empresa;
        if (!string.IsNullOrEmpty(request.Descripcion)) offer.Descripcion = request.Descripcion;
        if (request.Salario.HasValue) offer.Salario = request.Salario.Value;
        if (!string.IsNullOrEmpty(request.Ubicacion)) offer.Ubicacion = request.Ubicacion;
        if (request.Modalidad.HasValue) offer.Modalidad = (Modalidad)request.Modalidad.Value;
        if (!string.IsNullOrEmpty(request.Requisitos)) offer.Requisitos = request.Requisitos;
        if (request.Estado.HasValue) offer.Estado = (OfertaEstado)request.Estado.Value;

        await _unitOfWork.Ofertas.UpdateAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(offer);
    }

    public async Task<OfertaResponse> PublishOfertaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offer = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {id} not found");

        offer.Publicar();

        await _unitOfWork.Ofertas.UpdateAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(offer);
    }

    public async Task<OfertaResponse> CloseOfertaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offer = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {id} not found");

        offer.Cerrar();

        await _unitOfWork.Ofertas.UpdateAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OfertaResponse>(offer);
    }

    public async Task DeleteOfertaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offer = await _unitOfWork.Ofertas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {id} not found");

        await _unitOfWork.Ofertas.DeleteAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CandidaturaResponse>> GetCandidaturasByOfertaAsync(Guid ofertaId, CancellationToken cancellationToken = default)
    {
        var applications = await _unitOfWork.Candidaturas.GetByOfertaIdAsync(ofertaId, cancellationToken);
        return _mapper.Map<IEnumerable<CandidaturaResponse>>(applications);
    }

    public async Task<IEnumerable<CandidaturaResponse>> GetCandidaturasByJuniorAsync(string juniorId, CancellationToken cancellationToken = default)
    {
        var applications = await _unitOfWork.Candidaturas.GetByJuniorIdAsync(juniorId, cancellationToken);
        return _mapper.Map<IEnumerable<CandidaturaResponse>>(applications);
    }

    public async Task<CandidaturaResponse?> GetCandidaturaByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var application = await _unitOfWork.Candidaturas.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CandidaturaResponse?>(application);
    }

    public async Task<CandidaturaResponse> CreateCandidaturaAsync(CreateCandidaturaRequest request, CancellationToken cancellationToken = default)
    {
        var offer = await _unitOfWork.Ofertas.GetByIdAsync(request.OfertaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Oferta {request.OfertaId} not found");

        var alreadyExists = await _unitOfWork.Candidaturas.ExisteCandidaturaAsync(request.OfertaId, request.JuniorId, cancellationToken);
        if (alreadyExists)
            throw new InvalidOperationException($"Application already exists for junior {request.JuniorId} on offer {request.OfertaId}");

        var application = Candidatura.Create(
            ofertaId: request.OfertaId,
            juniorId: request.JuniorId,
            nombreJunior: request.NombreJunior,
            emailJunior: request.EmailJunior
        );

        await _unitOfWork.Candidaturas.AddAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(application);
    }

    public async Task<CandidaturaResponse> AdvanceToInterviewAsync(Guid candidaturaId, CancellationToken cancellationToken = default)
    {
        var application = await _unitOfWork.Candidaturas.GetByIdAsync(candidaturaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {candidaturaId} not found");

        application.AvanzarAEntrevista();

        await _unitOfWork.Candidaturas.UpdateAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(application);
    }

    public async Task<CandidaturaResponse> RejectCandidaturaAsync(Guid candidaturaId, CancellationToken cancellationToken = default)
    {
        var application = await _unitOfWork.Candidaturas.GetByIdAsync(candidaturaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {candidaturaId} not found");

        application.Rechazar();

        await _unitOfWork.Candidaturas.UpdateAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(application);
    }

    public async Task<CandidaturaResponse> HireCandidaturaAsync(Guid candidaturaId, CancellationToken cancellationToken = default)
    {
        var application = await _unitOfWork.Candidaturas.GetByIdAsync(candidaturaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {candidaturaId} not found");

        application.Contratar();

        await _unitOfWork.Candidaturas.UpdateAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CandidaturaResponse>(application);
    }

    public async Task DeleteCandidaturaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var application = await _unitOfWork.Candidaturas.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Candidatura {id} not found");

        await _unitOfWork.Candidaturas.DeleteAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

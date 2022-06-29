using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Repositories;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocumentResolutionByDocumentIdHandler : IQueryHandler<GetDocumentResolutionByDocumentIdQuery, GetDocumentResolutionByDocumentIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IDocumentResolutionService _documentResolutionService;

    public GetDocumentResolutionByDocumentIdHandler(IMapper mapper, IDocumentResolutionService documentResolutionService)
    {
        _mapper = mapper;
        _documentResolutionService = documentResolutionService;
    }
    public async Task<GetDocumentResolutionByDocumentIdResponse> Handle(GetDocumentResolutionByDocumentIdQuery request, CancellationToken cancellationToken)
    {
        var documentResolution = await _documentResolutionService.FindByDocumentIdAsync(request.DocumentId, cancellationToken);

        return _mapper.Map<GetDocumentResolutionByDocumentIdResponse>(documentResolution);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocument.Queries.GetByRegistrationNumber;

public class GetInternalDocumentByRegistrationNumberHandler
    : IQueryHandler<GetInternalDocumentByRegistrationNumberQuery, List<GetInternalDocumentByRegistrationNumberResponse>>
{
    private readonly IMapper _mapper;
    private readonly IDocumentsQueryService _queryService;

    public GetInternalDocumentByRegistrationNumberHandler(IMapper mapper, IDocumentsQueryService queryService)
    {
        _mapper = mapper;
        _queryService = queryService;
    }
    public async Task<List<GetInternalDocumentByRegistrationNumberResponse>> Handle(GetInternalDocumentByRegistrationNumberQuery request, CancellationToken cancellationToken)
    {
        var result = await _queryService.GetInternalDocsByRegistrationNumberAndYear(request.RegistrationNumber, DateTime.UtcNow.Year, cancellationToken);
        return _mapper.Map<List<GetInternalDocumentByRegistrationNumberResponse>>(result);
    }
}
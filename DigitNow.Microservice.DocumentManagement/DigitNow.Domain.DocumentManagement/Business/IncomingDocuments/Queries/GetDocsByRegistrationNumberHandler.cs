using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocsByRegistrationNumberHandler : IQueryHandler<GetDocsByRegistrationNumberQuery, GetDocsByRegistrationNumberResponse>
{
    private readonly IMapper _mapper;
    private readonly IDocumentService _documentService;

    public GetDocsByRegistrationNumberHandler(IMapper mapper, IDocumentService documentService)
    {
        _mapper = mapper;
        _documentService = documentService;
    }
    public async Task<GetDocsByRegistrationNumberResponse> Handle(GetDocsByRegistrationNumberQuery request, CancellationToken cancellationToken)
    {
        request.Year = request.Year == 0 ? DateTime.Now.Year : request.Year;

        var documents = await _documentService.FindAsync(x => x.RegistrationNumber == request.RegistrationNumber && request.Year == request.Year, cancellationToken);

        return _mapper.Map<GetDocsByRegistrationNumberResponse>(documents);
    }
}
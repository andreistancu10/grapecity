using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Archive.Queries;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Archive;

[Authorize]
[ApiController]
[Route("api/archive/operational")]
public class ArchiveController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly IExportService<GetDocumentResponse> _exportService;

    public ArchiveController(IMediator mediator, IMapper mapper, IIdentityService identityService, IExportService<GetDocumentResponse> exportService)
    {
        _mediator = mediator;        
        _mapper = mapper;
        _identityService = identityService;
        _exportService = exportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetArchivedDocumentsAsync(CancellationToken cancellationToken)
    {
        var getArchivedDocumentsQuery = _mapper.Map<GetArchivedDocumentsQuery>(Request);

        return await _mediator.Send(getArchivedDocumentsQuery, cancellationToken) switch
        {
            var result => Ok(result)
        };
    }
}
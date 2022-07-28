using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Archive.Queries;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Archive;

[Authorize]
[ApiController]
[Route("api/archive")]
public class ArchiveController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public ArchiveController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;        
        _mapper = mapper;
    }

    [HttpPost("operational/get-documents")]
    public async Task<ActionResult<List<GetDocumentsResponse>>> GetDocumentsAsync([FromBody] GetDocumentsRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetDocumentsOperationalArchiveQuery>(request);
        return Ok(await _mediator.Send(query, cancellationToken));
    }
}
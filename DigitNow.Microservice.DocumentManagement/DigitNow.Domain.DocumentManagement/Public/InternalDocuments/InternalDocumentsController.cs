using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments;

//[Authorize]
[ApiController]
[Route("api/internal-documents")]
public class InternalDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public InternalDocumentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var query = new GetInternalDocumentByIdQuery { Id = id };

        return await _mediator.Send(query, cancellationToken) switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> CreateInternalDocument([FromBody] CreateInternalDocumentRequest request)
    {
        var command = _mapper.Map<CreateInternalDocumentCommand>(request);

        return CreateResponse(await _mediator.Send(command));
    }
}
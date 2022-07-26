using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments;

[Authorize]
[ApiController]
[Route("api/incoming-documents")]
public class IncomingDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IncomingDocumentsController(
        IMediator mediator, 
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var query = new GetIncomingDocumentByIdQuery { Id = id };

        return await _mediator.Send(query, cancellationToken) switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> CreateIncomingDocument([FromBody] CreateIncomingDocumentRequest request)
    {
        var command = _mapper.Map<CreateIncomingDocumentCommand>(request);

        return CreateResponse(await _mediator.Send(command));
    }


    [HttpGet("generate-registration-proof/{id}")]
    public async Task<IActionResult> GetPdf([FromRoute] int id, CancellationToken cancellationToken)
    {
        
        var response = await _mediator.Send(new GetRegistrationProofQuery
        {
            Id = id
        }, cancellationToken);

        if(response == null)
        {
            return NotFound();
        }

        return Ok(File(response.Content, response.ContentType, response.Name));
    }
}

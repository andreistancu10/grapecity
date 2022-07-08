using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments;

[Authorize]
[ApiController]
[Route("api/outgoing-documents")]
public class OutgoingDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OutgoingDocumentsController(
        IMediator mediator, 
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var query = new GetOutgoingDocumentByIdQuery { Id = id };

        return await _mediator.Send(query, cancellationToken) switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> CreateOutgoingDocument([FromBody] CreateOutgoingDocumentRequest request)
    {
        var command = _mapper.Map<CreateOutgoingDocumentCommand>(request);

        return CreateResponse(await _mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOutgoingDocument([FromRoute] int id, [FromBody] UpdateOutgoingDocumentRequest request, CancellationToken cancellationToken)
    {
        var updateOutgoingDocumentCommand = _mapper.Map<UpdateOutgoingDocumentCommand>(request);
        updateOutgoingDocumentCommand.Id = id;

        return CreateResponse(await _mediator.Send(updateOutgoingDocumentCommand, cancellationToken));
    }

    [HttpGet("generate-registration-proof/{id}")]
    public async Task<IActionResult> GetRegistrationProofPdf([FromRoute] int id, CancellationToken cancellationToken)
    {

        var response = await _mediator.Send(new GetRegistrationProofQuery
        {
            Id = id
        }, cancellationToken);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(File(response.Content, response.ContentType, response.Name));
    }
}
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/incoming-documents")]
public class IncomingDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IncomingDocumentsController(
        IMediator mediator, 
        IMapper mapper, 
        IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> CreateIncomingDocument([FromBody] CreateIncomingDocumentRequest request)
    {
        var command = _mapper.Map<CreateIncomingDocumentCommand>(request);

        return CreateResponse(await _mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncomingDocument([FromRoute] int id, [FromBody] UpdateIncomingDocumentRequest request, CancellationToken cancellationToken)
    {
        var updateIncomingDocumentCommand = _mapper.Map<UpdateIncomingDocumentCommand>(request);
        updateIncomingDocumentCommand.Id = id;

        return CreateResponse(await _mediator.Send(updateIncomingDocumentCommand, cancellationToken));
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

        return File(response.Content, response.ContentType, response.Name);
    }
}
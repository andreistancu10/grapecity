using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Commands.Create;
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

    public IncomingDocumentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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

    [HttpPost("{registrationNumber}/submit-workflow")]
    public async Task<IActionResult> SubmitWorkflowDecision([FromRoute] int registrationNumber, [FromBody] CreateWorkflowDecisionRequest request)
    {
        var command = _mapper.Map<CreateWorkflowDecisionCommand>(request);
        command.RegistrationNumber = registrationNumber;

        return CreateResponse(await _mediator.Send(command));
    }
}

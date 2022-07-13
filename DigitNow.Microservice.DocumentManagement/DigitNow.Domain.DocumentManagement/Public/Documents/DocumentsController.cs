using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Documents.Commands.SetDocumentsResolution;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;
using DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber;
using DigitNow.Domain.DocumentManagement.Public.Documents.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowInformation;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowHistoryByDocumentId;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments;

[Authorize]
[ApiController]
[Route("api/documents")]
public class DocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DocumentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("resolution/{documentId}")]
    public async Task<IActionResult> GetDocumentResolutionAsync([FromRoute] int documentId)
    {
        return await _mediator.Send(new GetDocumentResolutionByDocumentIdQuery { DocumentId = documentId })
            switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }

    [HttpPost("resolution")]
    public async Task<IActionResult> SetDocumentsResolutionAsync([FromBody] SetDocumentsResolutionRequest request)
    {
        var query = _mapper.Map<SetDocumentsResolutionQuery>(request);

        return await _mediator.Send(query)
            switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetByRegistrationNumber([FromQuery] int registrationNumber, [FromQuery] int year)
    {
        return await _mediator.Send(new GetDocsByRegistrationNumberQuery { RegistrationNumber = registrationNumber, Year = year })
            switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }


    [HttpGet("workflow-information/{documentId}")]
    public async Task<IActionResult> GetWorkflowInformation([FromRoute] long documentId)
    {
        return await _mediator.Send(new GetWorkflowInformationByDocumentIdQuery { DocumentId = documentId })
            switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }

    [HttpPost("{id}/submit-workflow")]
    public async Task<IActionResult> SubmitWorkflowDecision([FromRoute] int id, [FromBody] CreateWorkflowDecisionRequest request)
    {
        var command = _mapper.Map<CreateWorkflowDecisionCommand>(request);
        command.DocumentId = id;

        return CreateResponse(await _mediator.Send(command));
    }

    [HttpGet("workflow-history/{documentId}")]
    public async Task<IActionResult> GetWorkflowHistory([FromRoute] long documentId)
    {
        return await _mediator.Send(new GetWorkflowHistoryByDocumentIdQuery { DocumentId = documentId })
            switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }
}
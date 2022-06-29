﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;
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

    public OutgoingDocumentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
}
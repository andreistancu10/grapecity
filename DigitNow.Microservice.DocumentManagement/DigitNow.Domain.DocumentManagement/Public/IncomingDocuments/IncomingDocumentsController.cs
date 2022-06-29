﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments;

[Authorize]
[Route("api/incoming-documents")]
public class IncomingDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDocumentPdfGeneratorService _documentPdfGeneratorService;

    public IncomingDocumentsController(
        IMediator mediator, 
        IMapper mapper, 
        IHttpContextAccessor httpContextAccessor, 
        IDocumentPdfGeneratorService documentPdfGeneratorService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _documentPdfGeneratorService = documentPdfGeneratorService;
    }
    private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost]
    public async Task<IActionResult> CreateIncomingDocument([FromBody] CreateIncomingDocumentRequest request)
    {
        var command = _mapper.Map<CreateIncomingDocumentCommand>(request);
        command.User = GetUserId();

        return CreateResponse(await _mediator.Send(command));
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

        var file = await _documentPdfGeneratorService.GenerateIncomingDocRegistrationProofPdfAsync(new DocumentPdfDetails
        {
            IssuerName = response.IssuerName,
            RegistrationDate = response.RegistrationDate,
            RegistrationNumber = response.RegistrationNumber,
            ResolutionPeriod = response.ResolutionPeriod,
            DocumentType = "Cerere",
            CityHall = "Primaria Bucuresti",
            InstitutionHeader = "Primaria Bucuresti"
        });
        return File(file.Content, file.ContentType, file.Name);
    }
}
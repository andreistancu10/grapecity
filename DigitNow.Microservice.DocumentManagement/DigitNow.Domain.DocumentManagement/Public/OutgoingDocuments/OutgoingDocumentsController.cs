using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetByRegistrationNumberAndYear;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments;

[Authorize]
[Route("api/outgoing-documents")]
public class OutgoingDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDocumentPdfGeneratorService _documentPdfGeneratorService;


    public OutgoingDocumentsController(
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
    public async Task<IActionResult> CreateOutgoingDocument([FromBody] CreateOutgoingDocumentRequest request)
    {
        var command = _mapper.Map<CreateOutgoingDocumentCommand>(request);
        command.User = GetUserId();

        return CreateResponse(await _mediator.Send(command));
    }

    [HttpGet]
    public async Task<IActionResult> GetByRegistrationNumber([FromQuery] int registrationNumber, [FromQuery] int year)
    {
        return await _mediator.Send(new GetOutgoingDocumentsByRegistrationNumberAndYearQuery { RegistrationNumber = registrationNumber, Year = year })
            switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
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

        var file = await _documentPdfGeneratorService.GenerateOutgoingDocRegistrationProofPdfAsync(new DocumentPdfDetails
        {
            IssuerName = response.RecipientName,
            RegistrationDate = DateTime.Now,
            RegistrationNumber = response.RegistrationNumber,
            ResolutionPeriod = null,
            DocumentType = "Cerere",
            CityHall = "Primaria Bucuresti",
            InstitutionHeader = "Primaria Bucuresti"
        });
        return File(file.Content, file.ContentType, file.Name);
    }
}
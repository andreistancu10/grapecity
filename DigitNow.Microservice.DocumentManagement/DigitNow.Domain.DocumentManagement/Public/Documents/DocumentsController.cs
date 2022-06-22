using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber;
using DigitNow.Domain.DocumentManagement.Business.Documents.Queries.SetDocumentsResolution;
using DigitNow.Domain.DocumentManagement.Public.Documents.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments;

[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/documents")]
public class DocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DocumentsController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("resolution/{id}/{documentType}")]
    public async Task<IActionResult> GetDocumentResolution([FromRoute] int identifier, [FromRoute] int documentType)
    {
        throw new System.NotImplementedException();
    }

    [HttpPost("resolution")]
    public async Task<IActionResult> SetDocumentsResolution([FromBody] SetDocumentsResolutionRequest request)
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

}
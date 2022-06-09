using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries;
using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments;

[Authorize]
[Route("api/outgoing-documents")]
public class OutgoingDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OutgoingDocumentsController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
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
    public async Task<IActionResult> GetByRegistrationNumber([FromQuery] string registrationNumber)
    {
        return await _mediator.Send(new GetOutgoingByRegistrationNumberQuery { RegistrationNumber = registrationNumber })
            switch
        {
            null => NotFound(),
            var result => Ok(result)
        };
    }
}
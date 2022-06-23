using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments;

[Authorize]
[ApiController]
[Route("api/internal-documents")]
public class InternalDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InternalDocumentsController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost]
    public async Task<IActionResult> CreateInternalDocument([FromBody] CreateInternalDocumentRequest request)
    {
        var command = _mapper.Map<CreateInternalDocumentCommand>(request);
        command.User = GetUserId();

        return CreateResponse(await _mediator.Send(command));
    }
}
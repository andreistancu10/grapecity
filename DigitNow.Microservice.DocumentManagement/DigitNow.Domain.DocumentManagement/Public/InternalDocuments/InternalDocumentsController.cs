using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments;

[Authorize]
[ApiController]
[Route("api/internal-documents")]
public class InternalDocumentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public InternalDocumentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    [HttpPost]
    public async Task<IActionResult> CreateInternalDocument([FromBody] CreateInternalDocumentRequest request)
    {
        var command = _mapper.Map<CreateInternalDocumentCommand>(request);

        return CreateResponse(await _mediator.Send(command));
    }
}
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments
{
    [Authorize]
    [Route("api/incoming-documents")]
    public class IncomingDocumentsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IncomingDocumentsController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<IActionResult> GetByRegistrationNumber([FromQuery] string registrationNumber)
        {
            return await _mediator.Send(new GetDocsByRegistrationNumberQuery { RegistrationNumber = registrationNumber })
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}

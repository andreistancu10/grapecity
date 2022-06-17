using DigitNow.Domain.DocumentManagement.Business.ContactDetails.Queries;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.ContactDetails;

[Authorize]
[Route("api/contact-details")]
public class ContactDetailsController : ApiController
{
    private readonly IMediator _mediator;

    public ContactDetailsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByRegistrationNumber([FromQuery] string identificationNumber)
    {
        return await _mediator.Send(new GetContactDetailsByIdentificationNumberQuery { IdentificationNumber = identificationNumber })
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }

}
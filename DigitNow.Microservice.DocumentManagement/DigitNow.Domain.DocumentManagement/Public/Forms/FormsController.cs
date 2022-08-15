using System;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Localization.Client;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Forms
{
    [Authorize]
    [ApiController]
    [Route("api/forms")]
    public class FormsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILocalizationManager _localizationManger;

        public FormsController(
            IMediator mediator,
            IMapper mapper,
            ILocalizationManager localizationManger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _localizationManger = localizationManger;
        }
    }
}
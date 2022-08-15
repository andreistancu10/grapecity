using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisterById;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters;
using DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters
{
    [Authorize]
    [ApiController]
    [Route("api/special-registers")]
    public class SpecialRegistersController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IExportService<SpecialRegisterExportViewModel> _exportService;

        public SpecialRegistersController(
            IMediator mediator,
            IMapper mapper,
            IExportService<SpecialRegisterExportViewModel> exportService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _exportService = exportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialRegisterAsync([FromBody] CreateSpecialRegisterRequest request)
        {
            return await _mediator.Send(_mapper.Map<CreateSpecialRegisterCommand>(request))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateSpecialRegisterAsync([FromRoute] long id, [FromBody] UpdateSpecialRegisterRequest request)
        {
            var command = _mapper.Map<UpdateSpecialRegisterCommand>(request);
            command.Id = id;

            return await _mediator.Send(command)
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetSpecialRegistersAsync([FromQuery] GetSpecialRegisterRequest request)
        {
            return await _mediator.Send(new GetSpecialRegistersQuery())
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetSpecialRegisterByIdAsync([FromRoute] long id)
        {
            return await _mediator.Send(new GetSpecialRegisterByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportSpecialRegistersAsExcelAsync([FromQuery] ExportSpecialRegisterRequest request)
        {
            var query = _mapper.Map<SpecialRegisterExportExcelQuery>(request);
            var result = await _mediator.Send(query);
            var fileResult = await _exportService.CreateExcelFile("RegistreSpeciale", "RegistreSpeciale", result);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }
    }
}
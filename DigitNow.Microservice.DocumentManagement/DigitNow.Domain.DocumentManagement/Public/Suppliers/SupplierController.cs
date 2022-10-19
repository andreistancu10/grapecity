using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Delete;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.Suppliers.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Suppliers
{
    [Authorize]
    [ApiController]
    [Route("api/supplier")]
    public class SupplierController : ApiController
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SupplierController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierRequest request)
        {
            var command = _mapper.Map<CreateSupplierCommand>(request);
            return CreateResponse(await _mediator.Send(command));
        }
      

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateSupplierRequest request)
        {
            var command = _mapper.Map<UpdateSupplierCommand>(request);
            return CreateResponse(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var command = new DeleteSupplierCommand(id);
            return await _mediator.Send(command)
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var query = new GetSupplierByIdQuery(id);
            return await _mediator.Send(query)
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }
    }
}

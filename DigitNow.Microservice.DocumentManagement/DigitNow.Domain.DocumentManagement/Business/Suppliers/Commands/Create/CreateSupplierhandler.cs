using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Create
{
    public class CreateSupplierhandler : ICommandHandler<CreateSupplierCommand, ResultObject>
    {
        private readonly IMapper _mapper;
        private readonly ISupplierService _supplierService;
        public CreateSupplierhandler(ISupplierService supplierService ,IMapper mapper)
        {
            _mapper = mapper; 
            _supplierService = supplierService;
        }
        public async Task<ResultObject> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var newSupplier = _mapper.Map<Supplier>(request);
            await _supplierService.AddAsync(newSupplier, cancellationToken);
            return ResultObject.Created(newSupplier.Id);


        }
    }
}

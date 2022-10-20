using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Delete
{
    public class DeleteSupplierhandler : ICommandHandler<DeleteSupplierCommand, ResultObject>
    {
     
        private readonly ISupplierService _supplierService;
        public DeleteSupplierhandler(ISupplierService supplierService )
        {
     
            _supplierService = supplierService;
        }
        public async Task<ResultObject> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
           
            await _supplierService.DeleteAsync(request.Id, cancellationToken);
            return ResultObject.Created(request.Id);

        }
    }
}

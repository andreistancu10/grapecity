using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Delete
{
    public class DeleteSupplierCommand : ICommand<ResultObject>
    {

        public long Id { get; set; }
        public DeleteSupplierCommand(long id)
        {
            Id = id;
        }
    }

}



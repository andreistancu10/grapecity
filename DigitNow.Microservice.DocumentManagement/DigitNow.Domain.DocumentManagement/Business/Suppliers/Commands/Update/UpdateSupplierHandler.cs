using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Update
{
    public class UpdateSupplierHandler : ICommandHandler<UpdateSupplierCommand, ResultObject>
    {
        private readonly IMapper _mapper;
        private readonly ISupplierService _supplierService;
        public UpdateSupplierHandler(ISupplierService supplierService, IMapper mapper)
        {
            _mapper = mapper;
            _supplierService = supplierService;
        }

        public async Task<ResultObject> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {

            var supplierToupdate = await _supplierService.GetByIdQuery(request.Id)
                                    .Include(item => item.LegalRepresentatives)
                                    .Include(item => item.RegisteredOfficeContactDetail)
                                    .Include(item => item.RegisteredWorkplaceContactDetail)
                                    .FirstOrDefaultAsync(cancellationToken);

            if (supplierToupdate == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The Suppllier with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.supplier.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            _mapper.Map(request, supplierToupdate);

            await _supplierService.UpdateAsync(supplierToupdate, cancellationToken);
            return ResultObject.Created(supplierToupdate.Id);

        }
    }
}

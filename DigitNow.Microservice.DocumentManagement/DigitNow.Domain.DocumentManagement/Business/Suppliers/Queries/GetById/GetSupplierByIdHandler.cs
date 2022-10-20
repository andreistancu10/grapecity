using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetById
{
    public class GetSupplierByIdHandler : IQueryHandler<GetSupplierByIdQuery, GetSupplierByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISupplierService _supplierService;

        public GetSupplierByIdHandler(IMapper mapper,
            ISupplierService supplierService)
        {
            _mapper = mapper;
            _supplierService = supplierService;
        }

        public async Task<GetSupplierByIdResponse> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierService.GetByIdQuery(request.Id)
                                  .Include(item => item.LegalRepresentatives)
                                  .Include(item => item.RegisteredOfficeContactDetail)
                                  .Include(item => item.RegisteredWorkplaceContactDetail)
                                  .FirstOrDefaultAsync(cancellationToken);

            if (supplier == null)
                return new GetSupplierByIdResponse();

            return _mapper.Map<GetSupplierByIdResponse>(supplier);
        }
    }
}

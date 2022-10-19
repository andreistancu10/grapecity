using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetFilteredSuppliers
{
    public class GetFilteredSuppliersHandler : IQueryHandler<GetFilteredSuppliersQuery, ResultObject>
    {
        private readonly IMapper _mapper;
        private readonly ISupplierService _supplierService;

        public GetFilteredSuppliersHandler(IMapper mapper,
            ISupplierService supplierService)
        {
            _mapper = mapper;
            _supplierService = supplierService;
        }
        public async Task<ResultObject> Handle(GetFilteredSuppliersQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _supplierService.CountAsync(request.Filter, cancellationToken);

            if (totalItems == 0) return ResultObject.Ok(GetEmptyPageSupplierResponse(request));

            var suppliers = await _supplierService.GetAllAsync(request.Filter,
               request.Page,
               request.Count,
               cancellationToken);

            var viewModel = _mapper.Map<List<SupplierViewModel>>( suppliers);

            return ResultObject.Ok(BuildFirstPageSupplierResponse(request, totalItems, viewModel));

        }


        private static GetFilteredSuppliersResponse BuildFirstPageSupplierResponse(GetFilteredSuppliersQuery query, long totalItems, List<SupplierViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetFilteredSuppliersResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }
        private static GetFilteredSuppliersResponse GetEmptyPageSupplierResponse(GetFilteredSuppliersQuery query)
        {
            return new GetFilteredSuppliersResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<SupplierViewModel>()
            };
        }

    }

   
}

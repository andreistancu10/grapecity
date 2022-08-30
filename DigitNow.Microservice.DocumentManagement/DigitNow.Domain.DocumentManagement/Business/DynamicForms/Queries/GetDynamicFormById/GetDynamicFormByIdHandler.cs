﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById
{
    public class GetDynamicFormByIdHandler : IQueryHandler<GetDynamicFormByIdQuery, DynamicFormViewModel>
    {
        private readonly IDynamicFormsServices _dynamicFormsService;

        public GetDynamicFormByIdHandler(
            IDynamicFormsServices dynamicFormsService)
        {
            _dynamicFormsService = dynamicFormsService;
        }

        public async Task<DynamicFormViewModel> Handle(GetDynamicFormByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dynamicFormsService.GetDynamicFormViewModelAsync(request.Id, cancellationToken);
        }
    }
}
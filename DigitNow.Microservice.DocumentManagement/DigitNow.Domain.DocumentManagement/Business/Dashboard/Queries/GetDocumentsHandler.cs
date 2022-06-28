using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsHandler : IQueryHandler<GetDocumentsQuery, GetDocumentsResponse>
{
    private readonly IMapper _mapper;
    private readonly IDashboardService _dashboardService;

    private GetDocumentsQuery _query;
    private int PreviousYear => DateTime.UtcNow.Year - 1;

    public GetDocumentsHandler(IMapper mapper, IDashboardService dashboardService)
    {
        _mapper = mapper;
        _dashboardService = dashboardService;
    }

    public Task<GetDocumentsResponse> Handle(GetDocumentsQuery query, CancellationToken cancellationToken)
    {
        _query = query;

        if (query.Filter == null)
        {
            return GetSimpleResponseAsync(query, cancellationToken);
        }
        return GetComplexResponseAsync(query, cancellationToken);
    }

    private async Task<GetDocumentsResponse> GetSimpleResponseAsync(GetDocumentsQuery query, CancellationToken cancellationToken)
    {
        var predicates = PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear);

        var totalItems = await _dashboardService.CountAllDocumentsAsync(predicates, cancellationToken);

        var documentViewModels = await _dashboardService.GetAllDocumentsAsync(
                x => x.CreatedAt.Year >= PreviousYear,
                query.Page,
                query.Count,
            cancellationToken);

        return new GetDocumentsResponse
        {
            TotalItems = totalItems,
            PageNumber = query.Page,
            PageSize = query.Count,
            TotalPages = totalItems / documentViewModels.Count,
            Documents = documentViewModels
        };
    }

    private async Task<GetDocumentsResponse> GetComplexResponseAsync(GetDocumentsQuery query, CancellationToken cancellationToken)
    {
        var filterModel = query.Filter;
        
        //var x1 = await _dashboardService.CountAllDocumentsAsync(PredicateFactory.CreatePredicatesList<Document>(x => t(x)),
        //    cancellationToken);

        var predicates = PredicateFactory.CreatePredicatesList<Document>(
                document => FilterByRegistryType(filterModel, document));
                //&&
                //FilterByRegistrationNo(filterModel, document)
                //&&
                //FilterByRegistrationDate(filterModel, document)
                //&&
                //FilterByDocumentType(filterModel, document)
                //&&
                //FilterByDocumentCategory(filterModel, document)
                //&&
                //FilterByDocumentState(filterModel, document);

        var totalItems = await _dashboardService.CountAllDocumentsAsync(predicates, cancellationToken);

        var foundDocuments = await _dashboardService.GetAllDocumentsAsync(
            predicates[0],
            query.Page,
            query.Count,
            cancellationToken);

        return new GetDocumentsResponse
        {
            TotalItems = totalItems,
            PageNumber = query.Page,
            PageSize = query.Count,
            TotalPages = totalItems / foundDocuments.Count,
            Documents = foundDocuments
        };
    }

    private bool t(Document arg)
    {
        if (string.IsNullOrEmpty(_query.Filter.RegistryType))
        {
            return true; //TODO: Ask about this
        }
        return true;
    }

    private bool FilterByRegistryType(DocumentsFilter filterModel, Document document)
    {
        if (string.IsNullOrEmpty(filterModel.RegistryType))
        {
            return true; //TODO: Ask about this
        }
        return true;
    }

    private bool FilterByRegistrationNo(DocumentsFilter filterModel, Document document)
    {
        var registrationNoFilter = filterModel.RegistrationNoFilter;
        if (registrationNoFilter != null)
        {
            return document.RegistrationNumber >= registrationNoFilter.From && document.RegistrationNumber <= registrationNoFilter.To;
        }
        return true;
    }

    private bool FilterByRegistrationDate(DocumentsFilter filterModel, Document document)
    {
        var registrationDateFilter = filterModel.RegistrationDateFilter;
        if (registrationDateFilter != null)
        {
            return document.RegistrationDate >= registrationDateFilter.From && document.RegistrationDate <= registrationDateFilter.To;
        }
        return true;
    }

    private bool FilterByDocumentType(DocumentsFilter filterModel, Document document)
    {
        return document.DocumentType == filterModel.DocumentType;
    }

    private bool FilterByDocumentCategory(DocumentsFilter filterModel, Document document)
    {
        if (filterModel.DocumentType == DocumentType.Incoming || filterModel.DocumentType == DocumentType.Outgoing)
        {
            // TODO: Ask about this
            return false;
        }
        else if (filterModel.DocumentType != DocumentType.Internal)
        {
            // TODO: Ask about this
            return false;
        }

        return true;
    }

    private bool FilterByDocumentState(DocumentsFilter filterModel, Document document)
    {
        if (filterModel.DocumentState != null)
        {
            // TODO: Ask about this
            return false;
        }
        return true;
    }

}
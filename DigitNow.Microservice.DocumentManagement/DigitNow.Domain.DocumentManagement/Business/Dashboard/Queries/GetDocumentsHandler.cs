using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsHandler : IQueryHandler<GetDocumentsQuery, ResultPagedList<GetDocumentResponse>>
{
    private readonly IDocumentService _documentService;
    private readonly IVirtualDocumentService _virtualDocumentService;
    private readonly IIdentityService _identityService;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly IMapper _mapper;

    private int PreviousYear => DateTime.UtcNow.Year - 1;

    public GetDocumentsHandler(
        IMapper mapper,
        IDocumentService documentService,
        IIdentityService identityService,
        IIdentityAdapterClient identityAdapterClient, 
        IVirtualDocumentService virtualDocumentService)
    {
        _documentService = documentService;
        _identityService = identityService;
        _identityAdapterClient = identityAdapterClient;
        _virtualDocumentService = virtualDocumentService;
        _mapper = mapper;
    }

    public async Task<ResultPagedList<GetDocumentResponse>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var allDocumentsCount = await CountAllDocumentsAsync(cancellationToken);

        var getDocumentsResponses = await GetAllDocumentsAsync(request.Page, request.Count, cancellationToken);

        var pageCount = allDocumentsCount / request.Count;

        if (getDocumentsResponses.Count % request.Count > 0)
        {
            pageCount += 1;
        }

        var header = new PagingHeader(
            allDocumentsCount,
            request.Page,
            request.Count,
            pageCount);

        return new ResultPagedList<GetDocumentResponse>(header, getDocumentsResponses);
    }

    private async Task<int> CountAllDocumentsAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);

        if (user.Roles.ToList().Contains((long)UserRole.Mayor))
        {
            return await _documentService.CountAllAsync(x => x.CreatedAt.Year >= PreviousYear, cancellationToken);
        }

        var relatedUserIds = await GetRelatedUserIdsASync(user, cancellationToken);

        return await _documentService.CountAllAsync(x =>
            x.CreatedAt.Year >= PreviousYear
            &&
            relatedUserIds.Contains(x.CreatedBy)
        , cancellationToken);

    }

    private async Task<List<GetDocumentResponse>> GetAllDocumentsAsync(int page, int count, CancellationToken cancellationToken)
    {
        var user = await _identityAdapterClient.GetUserByIdAsync(_identityService.GetCurrentUserId(), cancellationToken);
        if (user == null) throw new InvalidOperationException(); //TODO: Add not found exception

        List<Document> documents;

        if (user.Roles.ToList().Contains((long)UserRole.Mayor))
        {
            documents = await _documentService.FindAllQueryable(x => x.CreatedAt.Year >= PreviousYear)
                .OrderByDescending(x => x.RegistrationDate)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
        else
        {
            var relatedUserIds = await GetRelatedUserIdsASync(user, cancellationToken);

            documents = await _documentService.FindAllQueryable(x => x.CreatedAt.Year >= PreviousYear && relatedUserIds.Contains(x.CreatedBy))
                .OrderByDescending(x => x.RegistrationDate)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        return await MapDocumentsAsync(documents, cancellationToken);
    }

    private async Task<List<GetDocumentResponse>> MapDocumentsAsync(IEnumerable<Document> documents, CancellationToken cancellationToken)
    {
        var result = new List<GetDocumentResponse>();

        var incomingDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Incoming)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<IncomingDocument>(incomingDocuments, cancellationToken, c => c.WorkflowHistory));

        var internalDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Internal)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<InternalDocument>(internalDocuments, cancellationToken));

        var outgoingDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Outgoing)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<OutgoingDocument>(outgoingDocuments, cancellationToken, c => c.WorkflowHistory));

        return result;
    }

    private async Task<List<GetDocumentResponse>> MapChildDocumentAsync<T>(List<Document> documents, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        where T : VirtualDocument
    {
        var documentIds = documents.Select(x => x.Id).ToList();
        var virtualDocuments = await _virtualDocumentService.FindAllAsync(x => documentIds.Contains(x.DocumentId), cancellationToken, includes);
        var documentRegistry = documents.ToDictionary(x => x, y => virtualDocuments.Where(x => x.DocumentId == y.Id));
        var result = new List<GetDocumentResponse>();

        foreach (var (_, document) in documentRegistry)
        {
            foreach (var virtualDocument in document)
            {
                result.Add(_mapper.Map<T, GetDocumentResponse>(virtualDocument as T));
            }
        }

        return result;
    }

    private async Task<IEnumerable<long>> GetRelatedUserIdsASync(User user, CancellationToken cancellationToken)
    {
        if (user.Roles.ToList().Contains((long)UserRole.Functionary))
        {
            return new List<long> { user.Id };
        }

        if (user.Roles.ToList().Contains((long)UserRole.HeadOfDepartment))
        {
            var departmentId = user.Departments.FirstOrDefault();
            var departmentUsers = await _identityAdapterClient.GetUsersByDepartmentIdAsync(departmentId, cancellationToken);
            return departmentUsers.Users.Select(x => x.Id).ToList();
        }

        throw new InvalidOperationException(); //TODO: Add descriptive error
    }

    private async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.GetCurrentUserId();
        var user = await _identityAdapterClient.GetUserByIdAsync(currentUserId, cancellationToken);
        if (user == null) throw new InvalidOperationException($"User with identifier '{currentUserId}' cannot be retrieved!");

        return user;
    }
}
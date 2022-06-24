using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsHandler : IQueryHandler<GetDocumentsQuery, ResultPagedList<GetDocumentResponse>>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IDocumentService _documentService;
    private readonly IIdentityService _identityService;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly IMapper _mapper;

    private int PreviousYear => DateTime.UtcNow.Year - 1;

    public GetDocumentsHandler(DocumentManagementDbContext dbContext,
        IMapper mapper,
        IDocumentService documentService,
        IIdentityService identityService,
        IIdentityAdapterClient identityAdapterClient)
    {
        _dbContext = dbContext;
        _documentService = documentService;
        _identityService = identityService;
        _identityAdapterClient = identityAdapterClient;
        _mapper = mapper;
    }

    public async Task<ResultPagedList<GetDocumentResponse>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var getDocumentsResponses = await GetAllDocumentsAsync(request.Page, request.Count, cancellationToken);

        var header = new PagingHeader(
            getDocumentsResponses.Count,
            request.Page,
            request.Count,
            getDocumentsResponses.Count / request.Count);

        return new ResultPagedList<GetDocumentResponse>(header, getDocumentsResponses);
    }

    private async Task<List<GetDocumentResponse>> GetAllDocumentsAsync(int page, int count, CancellationToken cancellationToken)
    {
        var user = await _identityAdapterClient.GetUserByIdAsync(_identityService.GetCurrentUserId());
        if (user == null) throw new InvalidOperationException(); //TODO: Add not found exception

        var documents = default(List<Document>);

        if (user.Roles.ToList().Contains((long)UserRole.Mayor))
        {
            documents = await _documentService.FindAsync(x => x.CreatedAt.Year >= PreviousYear, cancellationToken);
        }
        else
        {
            var relatedUserIds = await GetRelatedUserIdsASync(user);

            documents = await _documentService.FindAsync(x =>
                x.CreatedAt.Year >= PreviousYear
                &&
                relatedUserIds.Contains(x.CreatedBy)
            , cancellationToken);
        }

        return await MapDocumentsAsync(documents, cancellationToken);
    }

    private async Task<List<GetDocumentResponse>> MapDocumentsAsync(IEnumerable<Document> documents, CancellationToken cancellationToken)
    {
        var result = new List<GetDocumentResponse>();

        var incomingDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Incoming)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<IncomingDocument>(incomingDocuments, cancellationToken));

        var internalDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Internal)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<InternalDocument>(internalDocuments, cancellationToken));

        var outogingDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Outgoing)            
            .ToList();
        result.AddRange(await MapChildDocumentAsync<OutgoingDocument>(outogingDocuments, cancellationToken));

        return result;
    }

    private async Task<List<GetDocumentResponse>> MapChildDocumentAsync<T>(List<Document> documents, CancellationToken cancellationToken)
        where T : VirtualDocument
    {
        var documentIds = documents.Select(x => x.Id).ToList();

        var virtualDocuments = await _dbContext.Set<T>()
            .Where(x => documentIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var documentRegistry = documents.ToDictionary(x => x, y => virtualDocuments.Where(x => x.DocumentId == y.Id));

        var result = new List<GetDocumentResponse>();
        foreach (var registryEntry in documentRegistry)
        {
            var document = registryEntry.Value;

            foreach (var virtualDocument in registryEntry.Value)
            {
                result.Add(_mapper.Map<T, GetDocumentResponse>(virtualDocument));
            }
        }
        return result;
    }

    private async Task<IEnumerable<long>> GetRelatedUserIdsASync(User user)
    {
        if (user.Roles.ToList().Contains((long)UserRole.Functionary))
        {
            return new List<long> { user.Id };
        }

        if (user.Roles.ToList().Contains((long)UserRole.HeadOfDepartment))
        {
            var departmentId = user.Departments.FirstOrDefault();
            var departmentUsers = await _identityAdapterClient.GetUsersByDepartmentIdAsync(departmentId);
            return departmentUsers.Users.Select(x => x.Id).ToList();
        }

        throw new InvalidOperationException(); //TODO: Add descriptive error
    }
}
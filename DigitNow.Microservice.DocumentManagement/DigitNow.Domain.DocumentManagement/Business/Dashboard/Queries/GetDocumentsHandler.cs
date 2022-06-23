using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsHandler : IQueryHandler<GetDocumentsQuery, ResultPagedList<GetDocumentResponse>>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly IMapper _mapper;

    public GetDocumentsHandler(DocumentManagementDbContext dbContext, IIdentityAdapterClient identityAdapterClient, IMapper mapper)
    {
        _dbContext = dbContext;
        _identityAdapterClient = identityAdapterClient;
        _mapper = mapper;
    }

    public async Task<ResultPagedList<GetDocumentResponse>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var previousYear = DateTime.UtcNow.Year - 1;

        if (!int.TryParse(request.UserId, out var userId))
        {
            ResultObject.Error(new ErrorMessage
            {
                Message = $"User {userId} invalid.",
                Parameters = new[] { nameof(request.UserId) },
                TranslationCode = "document-management.backend.get.validation.userInvalid"
            });
        }

        var user = await _identityAdapterClient.GetUserByIdAsync(userId);
        UserRole? userRole = null;

        if (user.Roles.Contains((long)UserRole.Mayor))
        {
            userRole = UserRole.Mayor;
        }
        else if (user.Roles.Contains((long)UserRole.HeadOfDepartment))
        {
            userRole = UserRole.HeadOfDepartment;
        }
        else if (user.Roles.Contains((long)UserRole.Functionary))
        {
            userRole = UserRole.Functionary;
        }

        if (userRole is null)
        {
            ResultObject.Error(new ErrorMessage
            {
                Message = $"Could not determine Role of user {userId}.",
                Parameters = new[] { nameof(request.UserId) },
                TranslationCode = "document-management.backend.get.validation.userRoleInvalid"
            });
        }

        var departmentId = user.Departments.FirstOrDefault();
        var documentsQuery = userRole switch
        {
            UserRole.Functionary => GetDocumentsAsFunctionary(userId, previousYear, request.Page, request.Count),
            UserRole.HeadOfDepartment => await GetDocumentsAsHeadOfDepartmentAsync((int)departmentId, previousYear, request.Page, request.Count),
            UserRole.Mayor => GetAllDocuments(previousYear, request.Page, request.Count)
        };

        var documents = await documentsQuery.ToListAsync(cancellationToken);

        var header = new PagingHeader(
            documents.Count,
            request.Page,
            request.Count,
            documents.Count / request.Count);

        return new ResultPagedList<GetDocumentResponse>(header, documents);
    }

    private IQueryable<GetDocumentResponse> GetAllDocuments(int previousYear, int page, int count)
    {
        //TODO refactor the queries after OutgoingDocuments, IncomingDocuments and InternalDocuments inherit a common base class

        var outgoingDocumentsQuery = _dbContext.OutgoingDocuments
            .Skip((page - 1) * count)
            .Take(count)
            .Where(c => c.CreationDate.Year >= previousYear)
            .Select(c => _mapper.Map<GetDocumentResponse>(c))
            .OrderBy(c => c.RegistrationNumber);

        var incomingDocumentsQuery = _dbContext.IncomingDocuments
            .Skip((page - 1) * count)
            .Take(count)
            .Where(c => c.RegistrationDate.Year >= previousYear)
            .Select(c => _mapper.Map<GetDocumentResponse>(c))
            .OrderBy(c => c.RegistrationNumber);

        var internalDocumentsQuery = _dbContext.InternalDocuments
            .Skip((page - 1) * count)
            .Take(count)
            .Where(c => c.CreationDate.Year >= previousYear)
            .Select(c => _mapper.Map<GetDocumentResponse>(c))
            .OrderBy(c => c.RegistrationNumber);

        return outgoingDocumentsQuery.Union(incomingDocumentsQuery).Union(internalDocumentsQuery);
    }

    private async Task<IQueryable<GetDocumentResponse>> GetDocumentsAsHeadOfDepartmentAsync(int departmentId, int previousYear, int page, int count)
    {
        var users = await _identityAdapterClient.GetUsersByDepartmentIdAsync(departmentId);
        var userIds = users.Users.Select(c => c.Id.ToString());

        return GetAllDocuments(previousYear, page, count).Where(c => userIds.Contains(c.User)); //TODO Is property user of Documents string or int?
    }

    private IQueryable<GetDocumentResponse> GetDocumentsAsFunctionary(int userId, int previousYear, int page, int count)
    {
        return GetAllDocuments(previousYear, page, count).Where(c => c.User == userId.ToString()); //TODO Is property user of Documents string or int?
    }
}
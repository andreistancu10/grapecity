using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetByRegistrationNumberAndYear;

public class GetOutgoingDocumentByRegistrationNumberAndYearHandler
    : IQueryHandler<GetOutgoingDocumentsByRegistrationNumberAndYearQuery, List<GetDocumentsByRegistrationNumberAndYearResponse>>
{
    private readonly IMapper _mapper;
    private readonly DocumentManagementDbContext _dbContext;

    public GetOutgoingDocumentByRegistrationNumberAndYearHandler(IMapper mapper, DocumentManagementDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
    public async Task<List<GetDocumentsByRegistrationNumberAndYearResponse>> Handle(GetOutgoingDocumentsByRegistrationNumberAndYearQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.OutgoingDocuments
            .Where(doc =>
                doc.RegistrationNumber == request.RegistrationNumber &&
                doc.RegistrationDate.Year == DateTime.Now.Year)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<GetDocumentsByRegistrationNumberAndYearResponse>>(result);
    }
}
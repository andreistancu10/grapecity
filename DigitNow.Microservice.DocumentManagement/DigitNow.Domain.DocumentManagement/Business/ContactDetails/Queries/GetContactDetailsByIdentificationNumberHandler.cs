using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.ContactDetails.Queries;

public class GetContactDetailsByIdentificationNumberHandler : IQueryHandler<GetContactDetailsByIdentificationNumberQuery, GetContactDetailsByIdentificationNumberResponse>
{
    private readonly IMapper _mapper;
    private readonly DocumentManagementDbContext _dbContext;
    public GetContactDetailsByIdentificationNumberHandler(IMapper mapper, DocumentManagementDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
    public async Task<GetContactDetailsByIdentificationNumberResponse> Handle(GetContactDetailsByIdentificationNumberQuery request, CancellationToken cancellationToken)
    {
        var doc = await _dbContext.IncomingDocuments
            .Include(doc => doc.ContactDetail)
            .FirstOrDefaultAsync(doc => doc.IdentificationNumber
                .Equals(request.IdentificationNumber), cancellationToken);

        return _mapper.Map<GetContactDetailsByIdentificationNumberResponse>(doc?.ContactDetail);
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries;

public class GetReportHandler : IQueryHandler<GetReportQuery, List<GetReportResponse>>
{
    private readonly DocumentManagementDbContext _dbContext;

    public GetReportHandler(DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<GetReportResponse>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        ReportFactory.Create(request.Type);

        _dbContext.OutgoingDocuments.Where(c => c.CreatedAt >= request.From && c.CreatedAt <= request.To);
    }
}

public class ReportFactory
{
    public static Report Create()
}

public class Report
{
}
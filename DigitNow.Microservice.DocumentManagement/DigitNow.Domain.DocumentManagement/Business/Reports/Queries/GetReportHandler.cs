using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries
{
    public class GetReportHandler : IQueryHandler<GetReportQuery, List<ReportViewModel>>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetReportHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<ReportViewModel>> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            IReportProcessor report = request.Type switch
            {
                ReportType.ExpiredDocuments => new ExpiredReportProcessor(_serviceProvider),
                ReportType.DocumentsToExpire => new ToExpireReportProcessor(_serviceProvider),
                _ => throw new NotImplementedException(),
            };

            return await report.GetDataAsync(request.FromDate, request.ToDate, cancellationToken);
        }
    }
}
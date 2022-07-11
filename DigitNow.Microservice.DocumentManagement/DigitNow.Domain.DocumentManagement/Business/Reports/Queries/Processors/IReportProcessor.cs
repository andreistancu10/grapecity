using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;

public interface IReportProcessor
{
    Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken);
}
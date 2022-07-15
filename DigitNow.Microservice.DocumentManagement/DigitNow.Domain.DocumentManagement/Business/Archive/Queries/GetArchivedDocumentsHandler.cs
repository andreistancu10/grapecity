using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Queries;

public class GetArchivedDocumentsHandler : IQueryHandler<GetArchivedDocumentsQuery, List<GetArchivedDocumentsResponse>>
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public GetArchivedDocumentsHandler(IDashboardService dashboardService, IDocumentMappingService documentMappingService)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
    }

    public async Task<List<GetArchivedDocumentsResponse>> Handle(GetArchivedDocumentsQuery request, CancellationToken cancellationToken)
    {
        return new List<GetArchivedDocumentsResponse>();
    }
}

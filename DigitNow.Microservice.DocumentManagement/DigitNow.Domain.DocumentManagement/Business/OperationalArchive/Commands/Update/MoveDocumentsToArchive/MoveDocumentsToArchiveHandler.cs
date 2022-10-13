using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchive
{
    public class MoveDocumentsToArchiveHandler : ICommandHandler<MoveDocumentsToArchiveCommand, ResultObject>
    {
        private const int ARCHIVE_DOCUMENTS_DAYS_COUNT = 180;

        private readonly DocumentManagementDbContext _dbContext;
        private readonly ILogger<MoveDocumentsToArchiveHandler> _logger;

        public MoveDocumentsToArchiveHandler(DocumentManagementDbContext dbContext, ILogger<MoveDocumentsToArchiveHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<ResultObject> Handle(MoveDocumentsToArchiveCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Archiving documents");

            var documents = await _dbContext.Documents
                .Where(x => !x.IsArchived)
                .Where(x => x.StatusModifiedAt.Date.AddDays(ARCHIVE_DOCUMENTS_DAYS_COUNT) < DateTime.Now.Date)
                .Where(x => x.Status == DocumentStatus.Finalized || (x.DocumentType == DocumentType.Internal && x.Status == DocumentStatus.InWorkMayorCountersignature))
                .ToListAsync(cancellationToken);

            if (!documents.Any())
            {
                _logger.LogInformation("No documents found to archive");
                return ResultObject.Ok();
            }

            foreach (var document in documents)
            {
                document.IsArchived = true;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Documents archived ({documents.Count}): {string.Join(',', documents.Select(x => x.Id))}");

            return ResultObject.Ok();
        }
    }
}

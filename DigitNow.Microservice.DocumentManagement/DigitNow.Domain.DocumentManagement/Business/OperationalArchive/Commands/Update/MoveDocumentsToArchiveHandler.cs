using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update
{
    public class MoveDocumentsToArchiveHandler : ICommandHandler<MoveDocumentsToArchiveCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly ILogger<MoveDocumentsToArchiveHandler> _logger;
        //TODO: revert back to month interval
        //private readonly int _archivingPeriod = 6;

        public MoveDocumentsToArchiveHandler(DocumentManagementDbContext dbContext, ILogger<MoveDocumentsToArchiveHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<ResultObject> Handle(MoveDocumentsToArchiveCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler called");
            var documents = await _dbContext.Documents
                .Where(x => !x.IsArchived)
                //.Where(x => x.StatusModifiedAt.Date.AddMonths(_archivingPeriod) < System.DateTime.Now.Date)
                .Where(x => x.Status == DocumentStatus.Finalized || (x.DocumentType == DocumentType.Internal && x.Status == DocumentStatus.InWorkCountersignature))
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"Docs to be moved to archive: { documents.Count }");

            foreach (var document in documents)
            {
                document.IsArchived = true;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ResultObject.Ok();
        }
    }
}

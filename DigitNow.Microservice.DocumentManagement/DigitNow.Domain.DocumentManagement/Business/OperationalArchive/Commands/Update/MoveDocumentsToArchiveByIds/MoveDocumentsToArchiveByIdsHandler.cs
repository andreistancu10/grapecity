using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchiveByIds
{
    public class MoveDocumentsToArchiveByIdsHandler : ICommandHandler<MoveDocumentsToArchiveByIdsCommand, ResultObject>
    {


        private readonly DocumentManagementDbContext _dbContext;
        private readonly ILogger<MoveDocumentsToArchiveByIdsHandler> _logger;

        public MoveDocumentsToArchiveByIdsHandler(DocumentManagementDbContext dbContext, ILogger<MoveDocumentsToArchiveByIdsHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<ResultObject> Handle(MoveDocumentsToArchiveByIdsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Archiving documents");

            var documents = await _dbContext.Documents
                .Where(x => !x.IsArchived)
                .Where(x=> request.DocumentIds.Contains(x.Id))            
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

﻿using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update
{
    public class MoveDocumentsToArchiveHandler : ICommandHandler<MoveDocumentsToArchiveCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        //TODO: revert back to month interval
        //private readonly int _archivingPeriod = 6;

        public MoveDocumentsToArchiveHandler(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultObject> Handle(MoveDocumentsToArchiveCommand request, CancellationToken cancellationToken)
        {
            var documents = await _dbContext.Documents
                .Where(x => !x.IsArchived)
                //.Where(x => x.StatusModifiedAt.Date.AddMonths(_archivingPeriod) < System.DateTime.Now.Date)
                .Where(x => x.Status == DocumentStatus.Finalized || (x.DocumentType == DocumentType.Internal && x.Status == DocumentStatus.InWorkCountersignature))
                .ToListAsync(cancellationToken);

            foreach (var document in documents)
            {
                document.IsArchived = true;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ResultObject.Ok();
        }
    }
}

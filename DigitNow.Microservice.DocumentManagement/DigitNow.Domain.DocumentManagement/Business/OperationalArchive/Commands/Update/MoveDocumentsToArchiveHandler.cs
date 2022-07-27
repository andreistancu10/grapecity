using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update
{
    public class MoveDocumentsToArchiveHandler : ICommandHandler<MoveDocumentsToArchiveCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public MoveDocumentsToArchiveHandler(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultObject> Handle(MoveDocumentsToArchiveCommand request, CancellationToken cancellationToken)
        {
            //TODO: use the actual archiving period after it if implemented
            var archivingPeriod = 6; 
            var documents = await  _dbContext.Documents
                .Where(
                    x => !x.IsArchived &&
                    x.StatusModifiedAt.Date.AddMonths(archivingPeriod) < System.DateTime.Now.Date &&
                    (x.Status == DocumentStatus.Finalized || (x.DocumentType == DocumentType.Internal && x.Status == DocumentStatus.InWorkCountersignature))
                )
                .ToListAsync();

            foreach (var document in documents)
            {
                document.IsArchived = true;
            }
            await _dbContext.SaveChangesAsync();
            return ResultObject.Ok();
        }
    }
}

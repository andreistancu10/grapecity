using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Historical.Commands.DeleteHistoricalArchiveDocument
{
    public class DeleteHistoricalArchiveDocumentHandler : ICommandHandler<DeleteHistoricalArchiveDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DeleteHistoricalArchiveDocumentHandler(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultObject> Handle(DeleteHistoricalArchiveDocumentCommand request, CancellationToken cancellationToken)
        {

            var document = await _dbContext.DynamicFormFillingLogs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (document == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The historical archive document with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.archive.historical.backend.deleteDocument.IdNotFound",
                    Parameters = new object[] { request.Id }
                });

            var dynamicFormFieldValue = await _dbContext.DynamicFormFieldValues.Where(x => x.DynamicFormFillingLogId == request.Id).ToArrayAsync(cancellationToken);
            _dbContext.RemoveRange(dynamicFormFieldValue);
            _dbContext.DynamicFormFillingLogs.Remove(document);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ResultObject.Ok();
        }
    }
}

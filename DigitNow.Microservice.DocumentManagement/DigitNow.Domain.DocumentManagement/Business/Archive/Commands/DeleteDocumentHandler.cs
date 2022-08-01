using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Commands
{
    public class DeleteDocumentHandler : ICommandHandler<DeleteDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        public DeleteDocumentHandler(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultObject> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _dbContext.Documents.FirstOrDefaultAsync(document => document.Id == request.DocumentId, cancellationToken);
            if (document == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The document with id '{request.DocumentId}' was not found.",
                    TranslationCode = "documentManagement.archive.operational.backend.deleteDocument.IdNotFound",
                    Parameters = new object[] { request.DocumentId }
                });

            document.IsArchived = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ResultObject.Ok();
        }
    }
}

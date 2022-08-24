using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices
{
    public interface IDocumentFileService
    {
        Task<UploadedFile> CreateAsync(UploadFileCommand command, Guid newGuid, string relativeFilePath, string absoluteFilePath, CancellationToken cancellationToken);
        Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task<List<UploadedFile>> FetchUploadedFilesForDocument(long documentId, CancellationToken cancellationToken);
    }

    public class DocumentFileService : IDocumentFileService
    {
        private readonly IUploadedFileService _uploadedFileService;
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentFileService(
            IUploadedFileService uploadedFileService,
            DocumentManagementDbContext dbContext)
        {
            _uploadedFileService = uploadedFileService;
            _dbContext = dbContext;
        }

        public async Task<UploadedFile> CreateAsync(UploadFileCommand command, Guid newGuid, string relativeFilePath, string absoluteFilePath,
            CancellationToken cancellationToken)
        {
            var uploadedFile = await _uploadedFileService.CreateAsync(command, newGuid, relativeFilePath, absoluteFilePath, cancellationToken);

            if (command.DocumentCategoryId != null)
            {
                var newDocumentFileMapping = new DocumentFileMapping
                {
                    DocumentCategoryId = (long)command.DocumentCategoryId,
                    UploadedFileMappingId = uploadedFile.UploadedFileMappingId
                };

                await _dbContext.DocumentFileMappings.AddAsync(newDocumentFileMapping, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return uploadedFile;
        }

        public async Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            var uploadedFileMappings = await _uploadedFileService.GetUploadedFileMappingsAsync(uploadedFileIds, TargetEntity.Document, cancellationToken);
            uploadedFileMappings.ForEach(c => c.TargetId = document.Id);
            _dbContext.UploadedFileMappings.UpdateRange(uploadedFileMappings);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            var currentlyPresentIds = document.DocumentUploadedFiles.Select(c => c.UploadedFileId).ToList();
            var uploadedFileIdsToRemove = currentlyPresentIds.Where(c => !uploadedFileIds.Contains(c));
            var documentUploadedFilesToRemove = document.DocumentUploadedFiles.Where(c => uploadedFileIdsToRemove.Contains(c.UploadedFileId));
            var idsToAdd = uploadedFileIds.Where(c => !currentlyPresentIds.Contains(c));
            _dbContext.UploadedFileMappings.RemoveRange(documentUploadedFilesToRemove);

            await UpdateUploadedFilesWithTargetIdAsync(idsToAdd, document, cancellationToken);
        }
        
        public Task<List<UploadedFile>> FetchUploadedFilesForDocument(long documentId, CancellationToken cancellationToken)
        {
            return _dbContext.UploadedFileMappings
                .AsNoTracking()
                .Include(c => c.UploadedFile)
                .ThenInclude(c => c.UploadedFileMapping)
                .Where(c => c.TargetId == documentId && c.TargetEntity == TargetEntity.Document)
                .Select(c => c.UploadedFile)
                .ToListAsync(cancellationToken);
        }
    }
}
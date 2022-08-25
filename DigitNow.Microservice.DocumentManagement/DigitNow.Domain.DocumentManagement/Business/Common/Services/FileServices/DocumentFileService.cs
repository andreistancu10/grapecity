using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices
{
    public interface IDocumentFileService
    {
        Task<DocumentFileModel> CreateAsync(DocumentFileModel documentFileModel, CancellationToken cancellationToken);
        Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task<List<DocumentFileModel>> FetchUploadedFilesForDocument(long documentId, CancellationToken cancellationToken);
    }

    public class DocumentFileService : IDocumentFileService
    {
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IMapper _mapper;
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentFileService(
            IUploadedFileService uploadedFileService,
            DocumentManagementDbContext dbContext,
            IMapper mapper)
        {
            _uploadedFileService = uploadedFileService;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<DocumentFileModel> CreateAsync(DocumentFileModel documentFileModel, CancellationToken cancellationToken)
        {
            var uploadedFile = await _uploadedFileService.CreateAsync(documentFileModel, cancellationToken);

            var newDocumentFileMapping = new DocumentFileMapping
            {
                DocumentCategoryId = documentFileModel.DocumentCategoryId,
                UploadedFileMappingId = uploadedFile.UploadedFileMappingId
            };

            await _dbContext.DocumentFileMappings.AddAsync(newDocumentFileMapping, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            documentFileModel.Id = uploadedFile.Id;

            return documentFileModel;
        }

        public async Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(
                uploadedFileIds,
                document.Id,
                TargetEntity.Document,
                cancellationToken);
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

        public async Task<List<DocumentFileModel>> FetchUploadedFilesForDocument(long documentId, CancellationToken cancellationToken)
        {
            var uploadedFiles = await _dbContext.UploadedFileMappings
                .AsNoTracking()
                .Include(c => c.UploadedFile)
                .ThenInclude(c => c.UploadedFileMapping)
                .Where(c => c.TargetId == documentId && c.TargetEntity == TargetEntity.Document)
                .Select(c => c.UploadedFile)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<DocumentFileModel>>(uploadedFiles);
        }
    }
}
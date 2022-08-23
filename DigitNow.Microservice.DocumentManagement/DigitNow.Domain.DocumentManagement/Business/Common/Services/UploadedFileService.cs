using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IUploadedFileService
    {
        Task<UploadedFile> CreateAsync(UploadFileCommand command, Guid newGuid, string relativeFilePath, string absoluteFilePath, CancellationToken cancellationToken);
        Task UpdateUploadedFilesWithTargetIdForDocumentAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task UpdateUploadedFilesWithTargetIdForObjectiveAsync(IEnumerable<long> uploadedFileIds, Objective objective, CancellationToken cancellationToken);
        Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids,
            TargetEntity targetEntity,
            CancellationToken cancellationToken);
        Task<List<UploadedFile>> FetchUploadedFilesForDocument(long documentId, CancellationToken cancellationToken);
    }

    public class UploadedFileService : IUploadedFileService
    {
        private readonly IMapper _mapper;
        private readonly DocumentManagementDbContext _dbContext;

        public UploadedFileService(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<UploadedFile> CreateAsync(
            UploadFileCommand command,
            Guid newGuid,
            string relativeFilePath,
            string absoluteFilePath,
            CancellationToken cancellationToken)
        {
            var newUploadedFile = _mapper.Map<UploadedFile>(command);
            newUploadedFile.Guid = newGuid;
            newUploadedFile.RelativePath = relativeFilePath;
            newUploadedFile.AbsolutePath = absoluteFilePath;

            await _dbContext.UploadedFiles.AddAsync(newUploadedFile, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var uploadedFileMapping = new UploadedFileMapping
            {
                UploadedFileId = newUploadedFile.Id,
                TargetEntity = command.TargetEntity,
            };

            if (command.TargetId != null)
            {
                uploadedFileMapping.TargetId = (long)command.TargetId;
            }

            await _dbContext.UploadedFileMappings.AddAsync(uploadedFileMapping, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            newUploadedFile.UploadedFileMappingId = uploadedFileMapping.Id;

            if (command.DocumentCategoryId != null)
            {
                var newDocumentFileMapping = new DocumentFileMapping
                {
                    DocumentCategoryId = (long)command.DocumentCategoryId,
                    UploadedFileMappingId = uploadedFileMapping.Id
                };

                await _dbContext.DocumentFileMappings.AddAsync(newDocumentFileMapping, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return newUploadedFile;
        }

        public async Task UpdateUploadedFilesWithTargetIdForDocumentAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            var uploadedFileMappings = await GetUploadedFileMappingsAsync(uploadedFileIds, TargetEntity.Document, cancellationToken);
            uploadedFileMappings.ForEach(c => c.TargetId = document.Id);
            _dbContext.UploadedFileMappings.UpdateRange(uploadedFileMappings);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUploadedFilesWithTargetIdForObjectiveAsync(IEnumerable<long> uploadedFileIds, Objective objective, CancellationToken cancellationToken)
        {
            var uploadedFileMappings = await GetUploadedFileMappingsAsync(uploadedFileIds, TargetEntity.Objective, cancellationToken);
            uploadedFileMappings.ForEach(c => c.TargetId = objective.Id);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _dbContext.UploadedFileMappings.UpdateRange(uploadedFileMappings);
        }

        public async Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            var currentlyPresentIds = document.DocumentUploadedFiles.Select(c => c.UploadedFileId).ToList();
            var uploadedFileIdsToRemove = currentlyPresentIds.Where(c => !uploadedFileIds.Contains(c));
            var documentUploadedFilesToRemove = document.DocumentUploadedFiles.Where(c => uploadedFileIdsToRemove.Contains(c.UploadedFileId));
            var idsToAdd = uploadedFileIds.Where(c => !currentlyPresentIds.Contains(c));
            _dbContext.UploadedFileMappings.RemoveRange(documentUploadedFilesToRemove);

            await UpdateUploadedFilesWithTargetIdForDocumentAsync(idsToAdd, document, cancellationToken);
        }

        public async Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids, TargetEntity targetEntity, CancellationToken cancellationToken)
        {
            return await _dbContext.UploadedFileMappings
                .Include(c => c.UploadedFile)
                .Where(c => ids.Contains(c.Id) && c.TargetEntity == targetEntity)
                .ToListAsync(cancellationToken);
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
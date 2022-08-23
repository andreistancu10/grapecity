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
        Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task CreateObjectiveUploadedFilesAsync(IEnumerable<long> uploadedFileIds, Objective objective, CancellationToken cancellationToken);
        Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids,
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

        public async Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            var uploadedFiles = await GetUploadedFileMappingsAsync(uploadedFileIds, cancellationToken);
            uploadedFiles.ForEach(c => c.TargetId = document.Id);
            _dbContext.UploadedFileMappings.UpdateRange(uploadedFiles);
        }

        public async Task CreateObjectiveUploadedFilesAsync(IEnumerable<long> uploadedFileIds, Objective objective, CancellationToken cancellationToken)
        {
            var uploadedFiles = await GetUploadedFileMappingsAsync(uploadedFileIds, cancellationToken);

            var objectiveUploadedFiles = uploadedFiles.Select(file => new ObjectiveUploadedFile
            {
                UploadedFile = file.UploadedFile,
                Objective = objective
            });

            await _dbContext.ObjectiveUploadedFiles.AddRangeAsync(objectiveUploadedFiles, cancellationToken);
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

        public async Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            return await _dbContext.UploadedFileMappings
                .Include(c => c.UploadedFile)
                .Where(c => ids.Contains(c.Id))
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
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices
{
    public interface IUploadedFileService
    {
        Task<UploadedFile> CreateAsync(StoredFileModel storedFileModel, CancellationToken cancellationToken);
        Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, long targetId, TargetEntity targetEntity, CancellationToken cancellationToken);
        Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids,
            TargetEntity targetEntity,
            CancellationToken cancellationToken);
        Task<List<UploadedFile>> FetchUploadedFiles(TargetEntity targetEntity, long targetId, CancellationToken cancellationToken);
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

        public async Task<UploadedFile> CreateAsync(StoredFileModel storedFileModel, CancellationToken cancellationToken)
        {
            var newUploadedFile = _mapper.Map<UploadedFile>(storedFileModel);

            await _dbContext.UploadedFiles.AddAsync(newUploadedFile, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var uploadedFileMapping = new UploadedFileMapping
            {
                UploadedFileId = newUploadedFile.Id,
                TargetEntity = storedFileModel.TargetEntity,
            };

            if (storedFileModel.TargetId != null)
            {
                uploadedFileMapping.TargetId = (long)storedFileModel.TargetId;
            }

            await _dbContext.UploadedFileMappings.AddAsync(uploadedFileMapping, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            newUploadedFile.UploadedFileMappingId = uploadedFileMapping.Id;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newUploadedFile;
        }

        public async Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, long targetId, TargetEntity targetEntity, CancellationToken cancellationToken)
        {
            var uploadedFileMappings = await GetUploadedFileMappingsAsync(uploadedFileIds, targetEntity, cancellationToken);
            uploadedFileMappings.ForEach(c => c.TargetId = targetId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _dbContext.UploadedFileMappings.UpdateRange(uploadedFileMappings);
        }

        public async Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids, TargetEntity targetEntity, CancellationToken cancellationToken)
        {
            return await _dbContext.UploadedFileMappings
                .Include(c => c.UploadedFile)
                .Where(c => ids.Contains(c.Id) && c.TargetEntity == targetEntity)
                .ToListAsync(cancellationToken);
        }


        public Task<List<UploadedFile>> FetchUploadedFiles(TargetEntity targetEntity, long targetId, CancellationToken cancellationToken)
        {
            return _dbContext.UploadedFileMappings
                .AsNoTracking()
                .Include(c => c.UploadedFile)
                .ThenInclude(c => c.UploadedFileMapping)
                .Where(c => c.TargetId == targetId && c.TargetEntity == targetEntity)
                .Select(c => c.UploadedFile)
                .ToListAsync(cancellationToken);
        }
    }
}
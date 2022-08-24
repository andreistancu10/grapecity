using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices
{
    public interface IObjectiveFileService
    {
        Task<UploadedFile> CreateAsync(UploadFileCommand command, Guid newGuid, string relativeFilePath, string absoluteFilePath, CancellationToken cancellationToken);
        Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Objective objective, CancellationToken cancellationToken);
        Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids, CancellationToken cancellationToken);
    }

    public class ObjectiveFileService : IObjectiveFileService
    {
        private readonly IUploadedFileService _uploadedFileService;
        private readonly DocumentManagementDbContext _dbContext;

        public ObjectiveFileService(IUploadedFileService uploadedFileService,
            DocumentManagementDbContext dbContext)
        {
            _uploadedFileService = uploadedFileService;
            _dbContext = dbContext;
        }

        public async Task<UploadedFile> CreateAsync(UploadFileCommand command, Guid newGuid, string relativeFilePath, string absoluteFilePath,
            CancellationToken cancellationToken)
        {
            return await _uploadedFileService.CreateAsync(command, newGuid, relativeFilePath, absoluteFilePath, cancellationToken);
        }

        public async Task UpdateUploadedFilesWithTargetIdAsync(IEnumerable<long> uploadedFileIds, Objective objective, CancellationToken cancellationToken)
        {
            var uploadedFileMappings = await _uploadedFileService.GetUploadedFileMappingsAsync(uploadedFileIds, TargetEntity.Objective, cancellationToken);
            uploadedFileMappings.ForEach(c => c.TargetId = objective.Id);
            _dbContext.UploadedFileMappings.UpdateRange(uploadedFileMappings);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<UploadedFileMapping>> GetUploadedFileMappingsAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            return await _uploadedFileService.GetUploadedFileMappingsAsync(ids, TargetEntity.Objective, cancellationToken);
        }
    }
}
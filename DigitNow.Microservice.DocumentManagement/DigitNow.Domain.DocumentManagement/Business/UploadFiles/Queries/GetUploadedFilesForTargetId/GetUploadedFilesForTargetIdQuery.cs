using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetUploadedFilesForTargetId
{
    public class GetUploadedFilesForTargetIdQuery : IQuery<List<FileViewModel>>
    {
        public TargetEntity TargetEntity { get; set; }
        public long TargetId { get; set; }

        public GetUploadedFilesForTargetIdQuery(TargetEntity targetEntity, long targetId)
        {
            TargetEntity = targetEntity;
            TargetId = targetId;
        }
    }
}
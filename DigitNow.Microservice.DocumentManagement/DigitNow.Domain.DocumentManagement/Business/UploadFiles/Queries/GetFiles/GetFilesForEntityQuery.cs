using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesForEntityQuery : IQuery<List<DocumentFileViewModel>>
    {
        public int TargetEntity { get; set; }
        public long TargetId { get; set; }

        public GetFilesForEntityQuery(int targetEntity, long targetId)
        {
            TargetEntity = targetEntity;
            TargetId = targetId;
        }
    }
}
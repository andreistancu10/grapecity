using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesQuery : IQuery<List<FileViewModel>>
    {
        public long DocumentId { get; set; }

        public GetFilesQuery(long documentId)
        {
            DocumentId = documentId;
        }
    }
}
using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesQuery : IQuery<List<GetFilesResponse>>
    {
        public long DocumentId { get; set; }

        public GetFilesQuery(long documentId)
        {
            DocumentId = documentId;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class DocumentUploadedFileDto
    {
        public long DocumentId { get; set; }
        public long UploadedFileId { get; set; }
    }
}

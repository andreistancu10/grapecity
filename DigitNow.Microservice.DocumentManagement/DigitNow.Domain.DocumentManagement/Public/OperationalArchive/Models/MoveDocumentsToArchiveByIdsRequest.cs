using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.OperationalArchive.Models
{
    public class MoveDocumentsToArchiveByIdsRequest
    {
        public List<long> DocumentIds { get; set; }

    }
}

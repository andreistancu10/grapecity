using HTSS.Platform.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class ExtendedEntity : Entity
    {
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }

        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
    }

    public class SoftExtendedEntity : ExtendedEntity
    {
        public bool IsDeleted { get; set; }
    }
}

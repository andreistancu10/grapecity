using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class VirtualDocument : IExtendedEntity
    {
        #region [ Entity ]

        public long Id { get; set; }

        #endregion

        #region [ IExtendedEntity ]

        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }

        #endregion
    }
}

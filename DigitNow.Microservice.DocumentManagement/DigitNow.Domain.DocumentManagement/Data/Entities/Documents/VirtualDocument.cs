using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class VirtualDocument : IExtendedEntity
    {
        #region [ Entity ]

        [Column(nameof(Id), Order = 1)]
        public long Id { get; set; }

        #endregion

        #region [ IExtendedEntity ]

        [Column(nameof(CreatedAt), Order = 2)]
        public DateTime CreatedAt { get; set; }

        [Column(nameof(CreatedBy), Order = 3)]
        public long CreatedBy { get; set; }

        [Column(nameof(ModifiedAt), Order = 4)]
        public DateTime ModifiedAt { get; set; }

        [Column(nameof(ModifiedBy), Order = 5)]
        public long ModifiedBy { get; set; }

        #endregion

        #region [ Parent Relationship ]

        [Column(nameof(DocumentId), Order = 6)]
        public long DocumentId { get; set; }

        public Document Document { get; set; }

        #endregion
    }
}

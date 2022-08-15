using HTSS.Platform.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class ExtendedEntity : Entity, IExtendedEntity
    {
        [Column(nameof(Id), Order = 1)]
        public new long Id { get; set; }

        [Column(nameof(CreatedAt), Order = 2)]
        public DateTime CreatedAt { get; set; }

        [Column(nameof(CreatedBy), Order = 3)]
        public long CreatedBy { get; set; }

        [Column(nameof(ModifiedAt), Order = 4)] 
        public DateTime ModifiedAt { get; set; }

        [Column(nameof(ModifiedBy), Order = 5)]
        public long ModifiedBy { get; set; }
    }

    public class SoftExtendedEntity : ExtendedEntity, ISoftExtendedEntity
    {
        [Column(nameof(DeletedAt), Order = 6)]    
        public DateTime DeletedAt { get; set; }

        [Column(nameof(DeletedBy), Order = 7)]
        public long DeletedBy { get; set; }

        [Column(nameof(IsDeleted), Order = 8)]
        public bool IsDeleted { get; set; }
    }
}

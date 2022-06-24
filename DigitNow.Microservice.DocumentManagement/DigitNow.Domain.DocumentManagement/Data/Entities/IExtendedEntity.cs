using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public interface IExtendedEntity
    {
        DateTime CreatedAt { get; set; }
        long CreatedBy { get; set; }

        DateTime ModifiedAt { get; set; }
        long ModifiedBy { get; set; }
    }

    public interface ISoftExtendedEntity : IExtendedEntity
    {
        bool IsDeleted { get; set; }
        long DeletedBy { get; set; }
    }
}

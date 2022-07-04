using HTSS.Platform.Core.Domain;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class ExtendedEntity : Entity, IExtendedEntity
{
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }

    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}

public class SoftExtendedEntity : ExtendedEntity, ISoftExtendedEntity
{
    public bool IsDeleted { get; set; }
    public DateTime DeletedAt { get; set; }
    public long DeletedBy { get; set; }    
}

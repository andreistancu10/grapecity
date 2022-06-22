using HTSS.Platform.Core.Domain;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

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

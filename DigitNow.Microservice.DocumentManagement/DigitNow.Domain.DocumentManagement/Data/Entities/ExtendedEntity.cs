using HTSS.Platform.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class ExtendedEntity : Entity, IExtendedEntity
{
    [Column("Id", Order = 1)]
    public new long Id { get; set; }

    [Column("CreatedAt", Order = 2)]
    public DateTime CreatedAt { get; set; }

    [Column("CreatedBy", Order = 3)]
    public long CreatedBy { get; set; }

    [Column("ModifiedAt", Order = 4)] 
    public DateTime ModifiedAt { get; set; }

    [Column("ModifiedBy", Order = 5)]
    public long ModifiedBy { get; set; }
}

public class SoftExtendedEntity : ExtendedEntity, ISoftExtendedEntity
{
    [Column("DeletedAt", Order = 6)]    
    public DateTime DeletedAt { get; set; }

    [Column("DeletedBy", Order = 7)]
    public long DeletedBy { get; set; }

    [Column("IsDeleted", Order = 8)]
    public bool IsDeleted { get; set; }
}

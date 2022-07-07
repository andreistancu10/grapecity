﻿using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

public class UploadedFile : ExtendedEntity
{
    public long DocumentCategoryId { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public string RelativePath { get; set; }
    public Guid Guid { get; set; }
}
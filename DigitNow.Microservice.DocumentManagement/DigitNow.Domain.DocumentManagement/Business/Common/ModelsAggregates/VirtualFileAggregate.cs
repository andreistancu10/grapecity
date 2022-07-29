using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    internal class VirtualFileAggregate
    {
        internal UploadedFile UploadedFile { get; set; }
        internal IReadOnlyList<UserModel> Users { get; set; }
        internal IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
    }
}
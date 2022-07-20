using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class VirtualFileAggregate
    {
        public UploadedFile UploadedFile { get; set; }
        internal IReadOnlyList<UserModel> Users { get; set; }
        internal IReadOnlyList<UploadedFileCategoryModel> Categories { get; set; }
    }
}
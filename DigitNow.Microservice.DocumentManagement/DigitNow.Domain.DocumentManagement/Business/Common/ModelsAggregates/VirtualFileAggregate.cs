using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    internal class VirtualFileAggregate
    {
        internal UploadedFile UploadedFile { get; set; }
        internal IReadOnlyList<UserModel> Users { get; set; }
    }
}
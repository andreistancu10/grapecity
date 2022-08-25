using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class DocumentFileModel : StoredFileModel
    {
        public long DocumentCategoryId { get; set; }

    }
}
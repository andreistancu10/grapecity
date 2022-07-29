using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.ConnectedDocuments
{
    public class ConnectedDocument : Entity
    {
        public long DocumentId { get; set; }

        #region [ References ]
        public Document Document { get; set; }

        #endregion
    }
}
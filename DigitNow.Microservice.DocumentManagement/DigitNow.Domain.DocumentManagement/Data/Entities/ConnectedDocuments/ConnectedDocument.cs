using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class ConnectedDocument : Entity
{
    public long DocumentId { get; set; }

    #region [ References ]
    public Document Document { get; set; }

    #endregion
}
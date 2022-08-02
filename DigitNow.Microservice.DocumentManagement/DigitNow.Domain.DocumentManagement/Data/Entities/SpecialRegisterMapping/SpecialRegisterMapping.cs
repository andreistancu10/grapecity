using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class SpecialRegisterMapping : ExtendedEntity
    {
        public long DocumentId { get; set; }
        public long SpecialRegisterId { get; set; }

        #region [ References ]

        public Document Document { get; set; }
        public SpecialRegister SpecialRegister { get; set; }

        #endregion
    }
}
namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class FormFillingLog : ExtendedEntity
    {
        public long FormId { get; set; }

        #region [ References ]

        public Form Form { get; set; }

        #endregion
    }
}
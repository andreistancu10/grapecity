namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Standard : ExtendedEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId { get; set; }
        public string Activity { get; set; }
        public long DepartmentId { get; set; }
        public DateTime Deadline { get; set; }
        public string Observations { get; set; }
        public string ModificationMotive { get; set; }

        #region [ References ]
        public List<StandardFunctionary> StandardFunctionaries { get; set; }
        #endregion
    }
}

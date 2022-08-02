using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class SpecialRegisterMappingModel
    {
        public long DocumentId { get; set; }
        public long SpecialRegisterId { get; set; }

        #region [ References ]
   
        public Document Document { get; set; }
        public SpecialRegister SpecialRegister { get; set; }
    
        #endregion
    }
}
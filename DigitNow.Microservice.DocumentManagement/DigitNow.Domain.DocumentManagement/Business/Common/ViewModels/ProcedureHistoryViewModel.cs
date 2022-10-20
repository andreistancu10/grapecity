using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class ProcedureHistoryViewModel
    {
        public long Id { get; set; }
        public string Edition { get; set; }
        public string Revision { get; set; }
        public BasicViewModel User { get; set; }
        public BasicViewModel Procedure { get; set; }
    }
}
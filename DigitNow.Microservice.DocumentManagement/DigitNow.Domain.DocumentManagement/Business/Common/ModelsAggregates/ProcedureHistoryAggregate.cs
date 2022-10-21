using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class ProcedureHistoryAggregate
    {
        public ProcedureHistory ProcedureHistory { get; set; }   
        public Procedure Procedure { get; set; }
        public IReadOnlyList<UserModel> Users { get; set; }
    }
}
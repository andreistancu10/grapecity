using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class SpecialRegisterFetcherContext : ModelFetcherContext
    {
        public IList<SpecialRegister> SpecialRegisters
        {
            get => this[nameof(SpecialRegisters)] as IList<SpecialRegister>;
            set => this[nameof(SpecialRegisters)] = value;
        }
    }
}
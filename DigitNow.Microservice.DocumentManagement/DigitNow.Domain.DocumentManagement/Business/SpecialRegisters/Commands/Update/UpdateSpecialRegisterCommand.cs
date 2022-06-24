﻿using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Update
{
    public class UpdateSpecialRegisterCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        public int DocumentType{ get; set; }
    }
}

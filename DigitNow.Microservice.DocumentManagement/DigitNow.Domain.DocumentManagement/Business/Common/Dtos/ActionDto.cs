using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class ActionDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public ScimState State { get; set; }
        public string Details { get; set; }
    }
}

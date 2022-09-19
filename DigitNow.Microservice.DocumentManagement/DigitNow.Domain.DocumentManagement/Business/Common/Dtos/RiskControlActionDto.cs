using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class RiskControlActionDto
    {
        public long? Id { get; set; }
        public string ControlMeasurement { get; set; }
        public string Deadline { get; set; }
    }
}

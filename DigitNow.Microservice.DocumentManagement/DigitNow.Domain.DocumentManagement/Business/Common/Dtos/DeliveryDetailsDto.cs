using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class DeliveryDetailDto
    {
        public int DeliveryMode { get; set; }
        public int DirectShipping { get; set; }
        public int Post { get; set; }
        public string OutboundNumber { get; set; }
    }
}

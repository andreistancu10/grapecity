using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class InternalDocumentCategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int ResolutionPeriod { get; set; }
    }
}

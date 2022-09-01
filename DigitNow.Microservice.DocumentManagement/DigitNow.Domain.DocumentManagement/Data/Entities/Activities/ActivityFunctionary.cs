using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Activities
{
    public class ActivityFunctionary : ExtendedEntity
    {
        public long ActivityId { get; set; }
        public long FunctionaryId { get; set; }

        public Activity Activity { get; set; }
    }
}

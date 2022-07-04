using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Models
{
    public class VirtualDocumentAggregate
    {
        public VirtualDocument Document { get; set; }
        public Dictionary<long, User> Users { get; set; }
        public Dictionary<long, object> Categories { get; set; }
        public Dictionary<long, object> InternalCategories { get; set; }
    }
}

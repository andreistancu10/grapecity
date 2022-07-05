using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    internal class UserModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }        
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }

        public IList<long> Roles { get; set; } = new List<long>();
        public IList<long> Departments { get; set; } = new List<long>();
    }
}

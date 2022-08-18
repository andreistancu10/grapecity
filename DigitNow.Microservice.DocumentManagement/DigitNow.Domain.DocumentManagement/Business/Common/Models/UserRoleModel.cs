using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class UserRoleModel
    {
        public long Id { get; set; }
        public Guid IdentityCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool IsSystemRole { get; set; }
    }
}

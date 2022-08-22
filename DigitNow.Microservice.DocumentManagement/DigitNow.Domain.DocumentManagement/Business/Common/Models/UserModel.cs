using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }        
        public string Email
        {
            get => UserName;
            set => UserName = value;
        }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }

        public IList<UserDepartmentModel> Departments { get; set; } = new List<UserDepartmentModel>();
        public IList<UserRoleModel> Roles { get; set; } = new List<UserRoleModel>();
    }
}

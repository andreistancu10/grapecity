using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Standards
{
    internal class StandardsPermissionsFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public UserModel CurrentUser { get; set; }
    }
}

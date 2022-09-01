using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectivesPermissions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives
{
    internal class SpecificObjectivesPermissionsFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public UserModel CurrentUser { get; set; }
        public SpecificObjectivePermissionsFilter SpecificObjectivePermissionsFilter { get; set; }
    }
}

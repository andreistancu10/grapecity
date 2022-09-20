﻿using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Risks
{
    internal class RiskPermissionFilterBuilder : DataExpressionFilterBuilder<Risk, RiskPermissionFilter>
    {
        public RiskPermissionFilterBuilder(IServiceProvider serviceProvider, RiskPermissionFilter filter) 
            : base(serviceProvider, filter) { }

        protected override void InternalBuild()
        {
            if (EntityFilter.UserPermissionsFilter != null)
            {
                BuildUserPermissionsFilter();
            }
        }
        private void BuildUserPermissionsFilter()
        {
            var departments = EntityFilter.UserPermissionsFilter.DepartmentIds;

            EntityPredicates.Add(x => departments.Contains(x.DepartmentId));
        }
    }
}
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
    internal class ActivitiesFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public UserModel CurrentUser { get; set; }
        public ActivityFilter ActivityFilter { get; set; }
    }
}
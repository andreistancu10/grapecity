using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class ActivityRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> Users => GetItems<DocumentsUsersFetcher, UserModel>();
        public IReadOnlyList<DepartmentModel> Departments => GetItems<GenericDepartmentsFetcher, DepartmentModel>();

        public ActivityRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
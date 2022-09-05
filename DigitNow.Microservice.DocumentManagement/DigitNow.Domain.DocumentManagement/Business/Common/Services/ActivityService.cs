using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActivityService
    {
        Task<List<Activity>> GetActivitiesAsync(ActivityFilter requestFilter, int requestPage, int requestCount, CancellationToken cancellationToken);
        Task<long> CountActivitiesAsync(ActivityFilter requestFilter, CancellationToken cancellationToken);
    }

    public class ActivityService : IActivityService
    {
        public Task<List<Activity>> GetActivitiesAsync(ActivityFilter requestFilter, int requestPage, int requestCount,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountActivitiesAsync(ActivityFilter requestFilter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
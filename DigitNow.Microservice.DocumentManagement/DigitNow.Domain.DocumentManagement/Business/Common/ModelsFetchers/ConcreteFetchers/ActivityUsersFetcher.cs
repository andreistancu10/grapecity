using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class ActivityUsersFetcher : ModelFetcher<UserModel, ActivitiesFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;

        public ActivityUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        protected override async Task<List<UserModel>> FetchInternalAsync(ActivitiesFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.Activities.Where(c => c.CreatedBy > 0).Select(x => x.CreatedBy).ToList();
            var modifiedByUsers = context.Activities.Where(c => c.ModifiedBy > 0 && c.ModifiedBy != null).Select(c => c.ModifiedBy).ToList();
            var modifiedTargetUsers = new List<long>();
            modifiedByUsers.ForEach(x =>
            {
                if (x != null)
                    modifiedTargetUsers.Add((long)x);
            });
            var targetUsers = new List<long>();
            targetUsers.AddRange(createdByUsers);
            targetUsers.AddRange(modifiedTargetUsers);
            targetUsers = targetUsers.Distinct().ToList();

            var usersList = await _authenticationClient.Users.GetUsersByFilterAsync(new GetUsersByFilterRequest(), cancellationToken);

            var relatedUsers = usersList.Users
                .Where(x => targetUsers.Contains(x.Id))
                .Select(x => new UserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Active = x.Active,
                    Email = x.UserName
                })
                .ToList();

            return relatedUsers;
        }
    }
}
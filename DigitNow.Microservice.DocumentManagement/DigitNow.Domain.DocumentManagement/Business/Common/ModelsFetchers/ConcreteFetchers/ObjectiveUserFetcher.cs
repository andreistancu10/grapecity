using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class ObjectiveUserFetcher : ModelFetcher<UserModel, ObjectivesFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;

        public ObjectiveUserFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        protected override async Task<List<UserModel>> FetchInternalAsync(ObjectivesFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.Objectives
                .Select(x => x.CreatedBy)
                .ToList();
            var modifiedByUsers = context.Objectives
                .Select(x => x.ModifiedBy)
                .ToList();

            var targetUsers = new List<long>();
            targetUsers.AddRange(createdByUsers);
            targetUsers.AddRange(modifiedByUsers);
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

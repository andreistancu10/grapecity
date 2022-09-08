using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class ActionUsersFetcher : ModelFetcher<UserModel, ActionsFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;

        public ActionUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        protected override async Task<List<UserModel>> FetchInternalAsync(ActionsFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.Actions.Where(c => c.CreatedBy > 0).Select(x => x.CreatedBy).ToList();
            var modifiedByUsers = context.Actions.Where(c => c.ModifiedBy > 0).Select(c => c.ModifiedBy).ToList();

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
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class GeneralObjectivesUsersFetcher: ModelFetcher<UserModel, GeneralObjectivesFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;

        public GeneralObjectivesUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        protected override async Task<List<UserModel>> FetchInternalAsync(GeneralObjectivesFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.GeneralObjectives
                .Select(x => x.CreatedBy)
                .Distinct()
                .ToList();

            var usersList = await _authenticationClient.Users.GetUsersByFilterAsync(new GetUsersByFilterRequest(), cancellationToken);
            
            var relatedUsers = usersList.Users
                .Where(x => createdByUsers.Contains(x.Id))
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

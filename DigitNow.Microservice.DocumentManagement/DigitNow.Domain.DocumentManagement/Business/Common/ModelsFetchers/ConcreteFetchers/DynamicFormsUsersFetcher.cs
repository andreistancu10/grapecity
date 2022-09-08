using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DynamicFormsUsersFetcher : ModelFetcher<UserModel, DynamicFormsFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;

        public DynamicFormsUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        protected async override Task<List<UserModel>> FetchInternalAsync(DynamicFormsFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.DynamicFormValues
                .Select(x => x.CreatedBy)
                .ToList();

            var usersList = await _authenticationClient.Users.GetUsersByFilterAsync(new GetUsersByFilterRequest(), cancellationToken);

            var relatedUsers = usersList.Users
                .Where(x => createdByUsers.Contains(x.Id))
                .Select(x => new UserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                })
                .ToList();

            return relatedUsers;
        }
    }
}

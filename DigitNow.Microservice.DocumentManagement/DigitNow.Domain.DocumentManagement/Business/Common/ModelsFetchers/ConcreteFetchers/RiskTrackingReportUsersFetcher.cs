using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class RiskTrackingReportUsersFetcher : ModelFetcher<UserModel, RiskTrackingReportFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;
        public RiskTrackingReportUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        protected override async Task<List<UserModel>> FetchInternalAsync(RiskTrackingReportFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.RiskTrackingReports
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
                })
                .ToList();
            return relatedUsers;
        }
    }
}

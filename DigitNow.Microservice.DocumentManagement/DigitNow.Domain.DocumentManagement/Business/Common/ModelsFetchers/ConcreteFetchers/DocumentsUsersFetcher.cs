using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DocumentsUsersFetcher : ModelFetcher<UserModel, DocumentsFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;

        public DocumentsUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        public override async Task<IReadOnlyList<UserModel>> FetchAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.Documents
                .Select(x => x.CreatedBy)
                .ToList();

            var usersList = await _authenticationClient.GetUsersWithExtensions(cancellationToken);

            var relatedUsers = usersList.UserExtensions
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

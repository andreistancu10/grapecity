using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DocumentsUsersFetcher : ModelFetcher<UserModel, DocumentsFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IIdentityAdapterClient _identityAdapterClient;

        public DocumentsUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
            _identityAdapterClient = serviceProvider.GetService<IIdentityAdapterClient>();
        }

        protected override async Task<List<UserModel>> FetchInternalAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.Documents
                .Select(x => x.CreatedBy)
                .ToList();

            var recipientsUsers = context.Documents
                .Where(x => x.Document != null && x.Document.RecipientId.HasValue)
                .Select(x => x.Document.RecipientId.Value)
                .ToList();

            var targetUsers = new List<long>();
            targetUsers.AddRange(createdByUsers);
            targetUsers.AddRange(recipientsUsers);
            targetUsers = targetUsers.Distinct().ToList();

            var usersList = await _identityAdapterClient.GetUsersAsync(cancellationToken);

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

        [Obsolete("This will be investigated in the future")]
        private async Task<List<UserModel>> FetchInternalAsync_Rpc(DocumentsFetcherContext context, CancellationToken cancellationToken)
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

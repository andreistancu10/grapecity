﻿using System;
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
    internal class UploadedFilesUsersFetcher : ModelFetcher<UserModel, UploadedFilesFetcherContext>
    {
        private readonly IAuthenticationClient _authenticationClient;

        public UploadedFilesUsersFetcher(IServiceProvider serviceProvider)
        {
            _authenticationClient = serviceProvider.GetService<IAuthenticationClient>();
        }

        protected override async Task<List<UserModel>> FetchInternalAsync(UploadedFilesFetcherContext context, CancellationToken cancellationToken)
        {
            var createdByUsers = context.UploadFiles
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
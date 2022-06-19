﻿using DigitNow.Adapters.MS.Identity.Poco;

namespace DigitNow.Adapters.MS.Identity
{
    public interface IIdentityAdapterClient
    {
        Task<UserList> GetUsersByDepartmentIdAsync(int id);
        Task<User> GetUserByIdAsync(int id);

    }

    public class IdentityAdapterClient : IIdentityAdapterClient
    {
        private readonly IdentityHttpClient _identityHttpClient;

        public IdentityAdapterClient(IdentityHttpClient identityHttpClient)
        {
            _identityHttpClient = identityHttpClient;
        }

        public Task<User> GetUserByIdAsync(int id) => 
            _identityHttpClient.GetAsync<User>($"api/userExtensions/{id}");
       

        public Task<UserList> GetUsersByDepartmentIdAsync(int id) =>
            _identityHttpClient.GetAsync<UserList>($"api/userExtensions/department/{id}");
    }
}

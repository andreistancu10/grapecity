using DigitNow.Adapters.MS.Identity.Poco;

namespace DigitNow.Adapters.MS.Identity
{
    public interface IIdentityAdapterClient
    {
        Task<UserList> GetUsersByDepartmentIdAsync(long id, CancellationToken cancellationToken);
        Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken);
        Task CreateContactDetailsAsync(ContactDetailDto contactDetail, CancellationToken cancellationToken);
    }

    public class IdentityAdapterClient : IIdentityAdapterClient
    {
        private readonly IdentityHttpClient _identityHttpClient;

        public IdentityAdapterClient(IdentityHttpClient identityHttpClient)
        {
            _identityHttpClient = identityHttpClient;
        }

        public Task CreateContactDetailsAsync(ContactDetailDto contactDetail, CancellationToken cancellationToken) =>
            _identityHttpClient.PostAsync($"api/contact-details/", contactDetail, cancellationToken);

        public Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken) => 
            _identityHttpClient.GetAsync<User>($"api/userExtensions/{id}", cancellationToken);
       

        public Task<UserList> GetUsersByDepartmentIdAsync(long id, CancellationToken cancellationToken) =>
            _identityHttpClient.GetAsync<UserList>($"api/userExtensions/department/{id}", cancellationToken);
    }
}

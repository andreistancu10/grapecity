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
            _identityHttpClient.PostAsync($"contact-details/", contactDetail, cancellationToken);

        public Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken) => 
            _identityHttpClient.GetAsync<User>($"userExtensions/{id}", cancellationToken);
       

        public Task<UserList> GetUsersByDepartmentIdAsync(long id, CancellationToken cancellationToken) =>
            _identityHttpClient.GetAsync<UserList>($"userExtensions/department/{id}", cancellationToken);
    }
}

using DigitNow.Adapters.MS.Identity.Poco;

namespace DigitNow.Adapters.MS.Identity
{
    public interface IIdentityAdapterClient
    {
        Task<UserList> GetUsersAsync(CancellationToken cancellationToken);
        Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken);
        Task CreateContactDetailsAsync(IdentityContactDetail contactDetail, CancellationToken cancellationToken);
        Task<LegalEntity> GetCurrentLegalEntityAsync(CancellationToken cancellationToken);
        Task<UserList> GetUsersByRoleAndDepartment(string roleCode, long departmentId, CancellationToken cancellationToken);
    }

    public class IdentityAdapterClient : IIdentityAdapterClient
    {
        private readonly IdentityHttpClient _identityHttpClient;

        public IdentityAdapterClient(IdentityHttpClient identityHttpClient)
        {
            _identityHttpClient = identityHttpClient;
        }

        public Task CreateContactDetailsAsync(IdentityContactDetail contactDetail, CancellationToken cancellationToken) =>
            _identityHttpClient.PostAsync($"contact-details/", contactDetail, cancellationToken);

        public Task<LegalEntity> GetCurrentLegalEntityAsync(CancellationToken cancellationToken) =>
            _identityHttpClient.GetAsync<LegalEntity>($"contact-details/get-legalentity", cancellationToken);

        public Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken) => 
            _identityHttpClient.GetAsync<User>($"userExtensions/{id}", cancellationToken);
       

        public Task<UserList> GetUsersAsync(CancellationToken cancellationToken) =>
            _identityHttpClient.GetAsync<UserList>($"userExtensions/users/", cancellationToken);

        public Task<UserList> GetUsersByRoleAndDepartment(string roleCode, long departmentId, CancellationToken cancellationToken) => 
            _identityHttpClient.GetAsync<UserList>($"userExtensions/role/{roleCode}/department/{departmentId}", cancellationToken);
    }
}

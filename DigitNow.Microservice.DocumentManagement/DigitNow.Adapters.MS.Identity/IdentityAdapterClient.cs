using DigitNow.Adapters.MS.Identity.Poco;

namespace DigitNow.Adapters.MS.Identity
{
    public interface IIdentityAdapterClient
    {
        Task<UserList> GetUsersByDepartmentIdAsync(long id, CancellationToken cancellationToken);
        Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken);
        Task CreateContactDetailsAsync(ContactDetailDto contactDetail, CancellationToken cancellationToken);
        Task<UserList> GetUsersAsync(CancellationToken cancellationToken);
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

        public async Task<UserList> GetUsersAsync(CancellationToken cancellationToken)
        {
            var response = await _identityHttpClient.GetAsync<Dictionary<string, object>>("api/userExtensions/filter?pageNumber=1&PageSize=1000", cancellationToken);
            if (!response.ContainsKey("items"))
            {
                throw new InvalidDataException("Could not find `items` key on retrieving users!");
            }

            var users = response["items"] as UserList;
            if (users != null)
            {
                throw new InvalidCastException($"Cannot cast to `{nameof(UserList)}`!");
            }

            return users;
        }            
    }
}

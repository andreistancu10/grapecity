using AutoMapper;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;

namespace DigitNow.Adapters.MS.Identity
{
    public interface IAuthenticationClientAdapter
    {
        Task<List<User>> GetUsersByRoleAndDepartmentAsync(string roleCode, long departmentId, CancellationToken token);
    }

    public class AuthenticationClientAdapter : IAuthenticationClientAdapter
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IMapper _mapper;

        public AuthenticationClientAdapter(IAuthenticationClient authenticationClient, IMapper mapper)
        {
            _authenticationClient = authenticationClient;
            _mapper = mapper;
        }

        public async Task<List<User>> GetUsersByRoleAndDepartmentAsync(string roleCode, long departmentId, CancellationToken token)
        {
            var usersResponse = await _authenticationClient.GetUsersByRoleAndDepartment(new GetUsersByRoleAndDepartmentRequest
            {
                RoleCode = roleCode,
                DepartmentId = departmentId,
            }, token);

            return _mapper.Map<List<User>>(usersResponse.Users);
        }
    }
}

using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUserById;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIdentityService
    {
        Task<UserModel> GetCurrentUserAsync(CancellationToken token);
        long GetCurrentUserId();
        bool TryGetCurrentUserId(out int userId);
        Task<RecipientType> GetCurrentUserFirstRoleAsync(CancellationToken token);
        Task<long> GetCurrentUserFirstDepartmentIdAsync(CancellationToken token);

        Task<UserModel> GetUserByIdAsync(long userId, CancellationToken token);

        Task<UserModel> GetMayorAsync(CancellationToken token);

        Task<UserModel> GetHeadOfDepartmentUserAsync(long departmentId, CancellationToken token);
        Task<UserModel> GetHeadOfDepartmentUserAsync(string departmentCode, CancellationToken token);

        Task<long> GetHeadOfDepartmentUserIdAsync(long departmentId, CancellationToken token);
        Task<long> GetHeadOfDepartmentUserIdAsync(string departmentCode, CancellationToken token);

        Task<IList<UserModel>> GetUsersWithinDepartmentAsync(long departmentId, CancellationToken token);
        Task<IList<UserModel>> GetUsersWithinDepartmentAsync(string departmentCode, CancellationToken token);
    }

    public class Test : IGetUserByIdRequest
    {
        public long Id { get; set; }
    }

    public class IdentityService : IIdentityService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public IdentityService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICatalogAdapterClient catalogAdapterClient,
            IAuthenticationClient authenticationClient)
        {
            _mapper = mapper;
            _httpContextAccesor = httpContextAccessor;
            _catalogAdapterClient = catalogAdapterClient;
            _authenticationClient = authenticationClient;
        }

        public async Task<UserModel> GetCurrentUserAsync(CancellationToken token)
        {
            var currentUserId = GetCurrentUserId();

            var getUserByIdResponse = await _authenticationClient.Users.GetUserByIdAsync(new Test { Id = currentUserId }, token);
            if (getUserByIdResponse == null)
                throw new InvalidOperationException($"User with identifier '{currentUserId}' was not found!");

            return _mapper.Map<UserModel>(getUserByIdResponse.User);
        }

        public long GetCurrentUserId()
        {
            if (!TryGetCurrentUserId(out int userId))
            {
                throw new UnauthorizedAccessException("UserId is not attached on the request!");
            }
            return userId;
        }

        public bool TryGetCurrentUserId(out int userId)
        {
            var userIdClaim = _httpContextAccesor.HttpContext.User.FindFirstValue("htss_uid");

            return int.TryParse(userIdClaim, out userId);
        }

        public async Task<RecipientType> GetCurrentUserFirstRoleAsync(CancellationToken token)
        {
            var user = await GetUserByIdAsync(GetCurrentUserId(), token);

            //TODO: refactor this BS
            if (user.Departments.Select(x => x.Id).Contains(UserDepartment.MayorDepartment.Id) 
                && 
                user.Roles.Select(x => x.Code).Contains(RecipientType.HeadOfDepartment.Code))
            {
                return RecipientType.Mayor;
            }

            var userRole = user.Roles.Select(x => x.Code).Intersect(RecipientType.ListOfTypes.Select(x => x.Code)).FirstOrDefault();

            return RecipientType.ListOfTypes.FirstOrDefault(role => role.Code == userRole) ?? new RecipientType { Id = 0 };
        }

        public async Task<UserModel> GetUserByIdAsync(long userId, CancellationToken token)
        {
            var response = await _authenticationClient.Users.GetUserByIdAsync(new GetUserByIdRequest
            {
                Id = userId
            }, token);

            return _mapper.Map<UserModel>(response.User);
        }

        public async Task<UserModel> GetMayorAsync(CancellationToken token)
        {
            var mayorDepartment = await _catalogAdapterClient.GetDepartmentByCodeAsync("cabinetPrimar", token);

            return await GetHeadOfDepartmentUserAsync(mayorDepartment.Id, token);
        }

        public async Task<UserModel> GetHeadOfDepartmentUserAsync(long departmentId, CancellationToken token)
        {
            var response = await _authenticationClient.Users.GetUsersByFilterAsync(new GetUsersByFilterRequest
            {
                RoleFilter = new GetUsersByRoleFilter { Code = RecipientType.HeadOfDepartment.Code },
                DepartmentFilter = new GetUsersByDepartmentFilter { Id = departmentId }
            }, token);

            // TODO: Can be found multiple head of department. Review this 
            var foundUser = response.Users.FirstOrDefault();

            return _mapper.Map<UserModel>(foundUser);
        }

        public async Task<IList<UserModel>> GetUsersWithinDepartmentAsync(long departmentId, CancellationToken token)
        {
            var response = await _authenticationClient.Users.GetUsersByFilterAsync(new GetUsersByFilterRequest
            {
                DepartmentFilter = new GetUsersByDepartmentFilter { Id = departmentId }
            }, token);

            return _mapper.Map<IList<UserModel>>(response.Users);
        }

        public async Task<long> GetHeadOfDepartmentUserIdAsync(long departmentId, CancellationToken token)
        {
            var foundUser = await GetHeadOfDepartmentUserAsync(departmentId, token);
            if (foundUser != null)
            {
                return foundUser.Id;
            }
            return default;
        }

        public async Task<UserModel> GetHeadOfDepartmentUserAsync(string departmentCode, CancellationToken token)
        {
            var foundDepartment = await _catalogAdapterClient.GetDepartmentByCodeAsync(departmentCode, token);

            return await GetHeadOfDepartmentUserAsync(foundDepartment.Id, token);
        }

        public async Task<long> GetHeadOfDepartmentUserIdAsync(string departmentCode, CancellationToken token)
        {
            var foundUser = await GetHeadOfDepartmentUserAsync(departmentCode, token);
            if (foundUser != null)
            {
                return foundUser.Id;
            }
            return default;
        }

        public async Task<IList<UserModel>> GetUsersWithinDepartmentAsync(string departmentCode, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByCodeAsync(departmentCode, token);

            return await GetUsersWithinDepartmentAsync(department.Id, token);
        }

        public async Task<long> GetCurrentUserFirstDepartmentIdAsync(CancellationToken token)
        {
            var user = await GetUserByIdAsync(GetCurrentUserId(), token);
            var department = user.Departments.First();

            return department.Id;
        }
    }
}

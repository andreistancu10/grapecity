﻿using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIdentityService
    {
        Task<User> GetCurrentUserAsync(CancellationToken token);
        long GetCurrentUserId();
        bool TryGetCurrentUserId(out int userId);
        Task<RecipientType> GetCurrentUserFirstRoleAsync(CancellationToken token);

        Task<User> FetchMayorAsync(CancellationToken token);
        Task<long> FetchMayorIdAsync(CancellationToken token);

        Task<User> GetHeadOfDepartmentUserAsync(long departmentId, CancellationToken token);
        Task<long> GetHeadOfDepartmentUserIdAsync(long departmentId, CancellationToken token);

        Task<User> GetHeadOfDepartmentUserAsync(string departmentCode, CancellationToken token);
        Task<long> GetHeadOfDepartmentUserIdAsync(string departmentCode, CancellationToken token);
    }

    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public IdentityService(
            IHttpContextAccessor httpContextAccessor,
            IIdentityAdapterClient identityAdapterClient,
            ICatalogAdapterClient catalogAdapterClient)
        {
            _httpContextAccesor = httpContextAccessor;
            _identityAdapterClient = identityAdapterClient;
            _catalogAdapterClient = catalogAdapterClient;
        }

        public Task<User> GetCurrentUserAsync(CancellationToken token)
        {
            return _identityAdapterClient.GetUserByIdAsync(GetCurrentUserId(), token);
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
            var userIdClaim = _httpContextAccesor.HttpContext.User.FindFirstValue("htss_uid"); ;

            return int.TryParse(userIdClaim, out userId);
        }

        public async Task<RecipientType> GetCurrentUserFirstRoleAsync(CancellationToken token)
        {
            var userId = GetCurrentUserId();
            var user = await _identityAdapterClient.GetUserByIdAsync(userId, token);

            var userRole = user.Roles.Intersect(RecipientType.ListOfTypes.Select(x => x.Code)).FirstOrDefault();

            return RecipientType.ListOfTypes.FirstOrDefault(role => role.Code == userRole) ?? new RecipientType { Id = 0 };
        }


        [Obsolete("Refactor this once the Identity & Catalog SDKs are completed")]
        public Task<User> FetchMayorAsync(CancellationToken token) =>
            GetHeadOfDepartmentUserAsync("cabinetPrimar", token); //TODO: Fix this BS

        [Obsolete("Refactor this once the Identity & Catalog SDKs are completed")]
        public async Task<long> FetchMayorIdAsync(CancellationToken token)
        {
            var foundUser = await FetchMayorAsync(token);
            if (foundUser != null)
            {
                return foundUser.Id;
            }
            return default;
        }

        [Obsolete("Refactor this once the Identity & Catalog SDKs are completed")]
        public async Task<User> GetHeadOfDepartmentUserAsync(long departmentId, CancellationToken token)
        {
            var usersResponse = await _identityAdapterClient.GetUsersAsync(token);
            var allUsers = usersResponse.Users;

            return allUsers.FirstOrDefault(u => u.Departments.Contains(departmentId) && u.Roles.Contains(RecipientType.HeadOfDepartment.Code));
        }

        [Obsolete("Refactor this once the Identity & Catalog SDKs are completed")]
        public async Task<long> GetHeadOfDepartmentUserIdAsync(long departmentId, CancellationToken token)
        {
            var foundUser = await GetHeadOfDepartmentUserAsync(departmentId, token);
            if (foundUser != null)
            {
                return foundUser.Id;
            }
            return default;
        }

        [Obsolete("Refactor this once the Identity & Catalog SDKs are completed")]
        public async Task<User> GetHeadOfDepartmentUserAsync(string departmentCode, CancellationToken token)
        {
            var foundDepartment = await _catalogAdapterClient.GetDepartmentByCodeAsync(departmentCode, token);

            return await GetHeadOfDepartmentUserAsync(foundDepartment.Id, token);
        }

        [Obsolete("Refactor this once the Identity & Catalog SDKs are completed")]
        public async Task<long> GetHeadOfDepartmentUserIdAsync(string departmentCode, CancellationToken token)
        {
            var foundUser = await GetHeadOfDepartmentUserAsync(departmentCode, token);
            if (foundUser != null)
            {
                return foundUser.Id;
            }
            return default;
        }
    }
}

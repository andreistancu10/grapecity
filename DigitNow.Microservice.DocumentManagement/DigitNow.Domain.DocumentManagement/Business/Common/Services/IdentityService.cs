using DigitNow.Adapters.MS.Identity;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIdentityService
    {
        long GetCurrentUserId();
        bool TryGetCurrentUserId(out int userId);
        Task<UserRole> GetCurrentUserRoleAsync(CancellationToken cancellationToken);
    }

    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IIdentityAdapterClient _identityAdapterClient;

        public IdentityService(IHttpContextAccessor httpContextAccessor, IIdentityAdapterClient identityAdapterClient)
        {
            _httpContextAccesor = httpContextAccessor;
            _identityAdapterClient = identityAdapterClient;
        }

        public long GetCurrentUserId()
        {
            return 1;
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

        public async Task<UserRole> GetCurrentUserRoleAsync(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var user = await _identityAdapterClient.GetUserByIdAsync(userId, cancellationToken);

            var roles = Enum.GetValues(typeof(UserRole));
            var rolesArray = roles.Cast<int>().ToArray();

            var userRole = user.Roles.Intersect(rolesArray.Select(item => (long)item).ToArray()).FirstOrDefault();

            return (UserRole)userRole;
        }
    }
}

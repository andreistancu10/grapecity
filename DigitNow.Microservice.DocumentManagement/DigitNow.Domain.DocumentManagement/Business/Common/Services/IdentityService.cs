using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIdentityService
    {
        long GetCurrentUserId();
        bool TryGetCurrentUserId(out int userId);
    }

    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccesor = httpContextAccessor;
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
    }
}

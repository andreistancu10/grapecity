using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIdentityService
    {
        long GetCurrentUserId();
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
            var nameIdentifier = _httpContextAccesor.HttpContext.User.FindFirstValue("htss_uid");;

            if (string.IsNullOrEmpty(nameIdentifier) || string.IsNullOrEmpty(nameIdentifier))
                throw new UnauthorizedAccessException("UserId is not attached on the request!");

            return Convert.ToInt64(nameIdentifier);
        }
    }
}

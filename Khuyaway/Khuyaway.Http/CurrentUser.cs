using System.Security.Claims;
using Khuyaway.Common;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Http;

internal class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid Id
        => Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            out var httpId)
            ? httpId
            : Guid.Empty;
}
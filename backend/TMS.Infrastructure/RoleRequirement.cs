using Microsoft.AspNetCore.Authorization;
using TMS.Core.Enums;

namespace TMS.Infrastructure
{
    public class RoleRequirement(Role[] roles)
        : IAuthorizationRequirement
    {
        public Role[] Roles { get; set; } = roles;
    }
}

using Microsoft.AspNetCore.Authorization;
using TMS.Core.Enums;

namespace TMS.Infrastructure
{
    public class PermissionRequirement(Permission[] permissions)
        : IAuthorizationRequirement
    {
        public Permission[] Permissions { get; set; } = permissions;
    }
}

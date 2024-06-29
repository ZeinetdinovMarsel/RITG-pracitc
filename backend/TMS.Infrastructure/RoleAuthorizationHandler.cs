using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using TMS.Application;
using TMS.Core.Abstractions;
using TMS.Core.Enums;

namespace TMS.Infrastructure
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RoleAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(
             c => c.Type == CustomClaims.UserId);

            if (userId is null || !Guid.TryParse(userId.Value, out var id))
            {
                return;
            }
            using var scope = _serviceScopeFactory.CreateScope();

            var roleService = scope.ServiceProvider
            .GetRequiredService<IRoleService>();

            var roles = await roleService.GetRolesAsync(id);
            if (roles.Intersect(requirement.Roles).Any())
            {
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}

using TMS.Core.Enums;

namespace TMS.Application
{
    public interface IPermissionService
    {
        Task<HashSet<Permission>> GetPermissionsAsync(Guid userId);
    }
}
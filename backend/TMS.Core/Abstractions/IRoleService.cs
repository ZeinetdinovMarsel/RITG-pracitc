using TMS.Core.Enums;

namespace TMS.Core.Abstractions
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync(Guid userId);
    }
}
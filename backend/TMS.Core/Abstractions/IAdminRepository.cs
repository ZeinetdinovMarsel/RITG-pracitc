using TMS.Core.Enums;
using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface IAdminRepository
    {
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, User tsk, Role role);
    }
}
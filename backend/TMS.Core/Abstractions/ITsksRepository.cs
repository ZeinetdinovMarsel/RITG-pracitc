using TMS.Core.Models;

namespace TMS.DataAccess.Repositories
{
    public interface ITsksRepository
    {
        Task<Guid> Create(Tsk task);
        Task<Guid> Delete(Guid id);
        Task<List<Tsk>> Get();
        Task<Guid> Update(Guid id, string title, string description, int assignedUserId, string priority, string status, DateTime startDate, DateTime endDate);
    }
}
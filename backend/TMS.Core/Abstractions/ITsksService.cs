using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface ITsksService
    {
        Task<Guid> CreateTsk(Tsk tsk);
        Task<Guid> DeleteTsk(Guid id);
        Task<List<Tsk>> GetAllTsks();
        Task<Guid> UpdateTsk(
            Guid id, 
            string title, 
            string comment,
            string assignedUserId, 
            string priority, 
            string status, 
            DateTime startDate, 
            DateTime endDate);
    }
}
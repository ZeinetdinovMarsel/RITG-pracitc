using TMS.Core.Models;

namespace TMS.Application.Services
{
    public interface ITsksService
    {
        Task<Guid> CreateTsk(Tsk tsk);
        Task<Guid> DeleteTsk(Guid id);
        Task<List<Tsk>> GetAllTsks();
        Task<Guid> UpdateTsk(
            Guid id, 
            string title, 
            string description, 
            int assignedUserId, 
            string priority, 
            string status, 
            DateTime startDate, 
            DateTime endDate);
    }
}
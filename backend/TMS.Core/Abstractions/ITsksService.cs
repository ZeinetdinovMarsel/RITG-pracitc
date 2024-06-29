using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface ITsksService
    {
        Task<Guid> CreateTsk(Tsk tsk);
        Task<Guid> DeleteTsk(Guid id);
        Task<(Tsk,string)> GetTskById(Guid id);
        Task<List<Tsk>> GetAllTsksById(Guid id);
        Task<Guid> UpdateTsk(Guid id, Tsk tsk);
        Task<Guid> ChangeTskStat(Guid id, Tsk tsk);
        Task<List<TskHistory>> GetTskHistoryById(Guid id);
    }
}
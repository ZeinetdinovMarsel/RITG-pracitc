using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface ITsksRepository
    {
        Task<Guid> Create(Tsk task);
        Task<Guid> Delete(Guid id);
        Task<List<Tsk>> GetAll(Guid id);
        Task<(Tsk,string)> Get(Guid id);
        Task<Guid> Update(Guid id, Tsk task);
        Task<Guid> ChangeStat(Guid id, Tsk tsk);
    }
}
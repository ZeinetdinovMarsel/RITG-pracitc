using TMS.Core.Models;
using TMS.Core.Abstractions;

namespace TMS.Application.Services
{
    public class TsksService : ITsksService
    {
        private readonly ITsksRepository _tsksRepository;
        public TsksService(ITsksRepository tsksRepository)
        {
            _tsksRepository = tsksRepository;
        }

        public async Task<(Tsk,string)> GetTskById(Guid id)
        {
            return await _tsksRepository.Get(id);
        }
        public async Task<List<Tsk>> GetAllTsksById(Guid id)
        {
            return await _tsksRepository.GetAll(id);
        }

        public async Task<Guid> CreateTsk(Tsk tsk)
        {
            return await _tsksRepository.Create(tsk);
        }

        public async Task<Guid> UpdateTsk(Guid id, Tsk tsk)
        {
            return await _tsksRepository.Update(id, tsk);
        }

        public async Task<Guid> ChangeTskStat(Guid id, Tsk tsk)
        {
            return await _tsksRepository.ChangeStat(id, tsk);
        }

        public async Task<Guid> DeleteTsk(Guid id)
        {
            return await _tsksRepository.Delete(id);
        }
    }
}

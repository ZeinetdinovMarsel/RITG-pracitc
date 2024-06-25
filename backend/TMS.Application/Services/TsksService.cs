﻿
using TMS.Core.Models;
using TMS.DataAccess.Repositories;

namespace TMS.Application.Services
{
    public class TsksService : ITsksService
    {
        private readonly ITsksRepository _tsksRepository;
        public TsksService(ITsksRepository tsksRepository)
        {
            _tsksRepository = tsksRepository;
        }

        public async Task<List<Tsk>> GetAllTsks()
        {
            return await _tsksRepository.Get();
        }

        public async Task<Guid> CreateTsk(Tsk tsk)
        {
            return await _tsksRepository.Create(tsk);
        }

        public async Task<Guid> UpdateTsk(Guid id, string title, string description, int assignedUserId, string priority, string status, DateTime startDate, DateTime endDate)
        {
            return await _tsksRepository.Update(id, title, description, assignedUserId, priority, status, startDate, endDate);
        }

        public async Task<Guid> DeleteTsk(Guid id)
        {
            return await _tsksRepository.Delete(id);
        }
    }
}
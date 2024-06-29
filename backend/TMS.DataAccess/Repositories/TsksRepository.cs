using Microsoft.EntityFrameworkCore;
using TMS.Core.Models;
using TMS.DataAccess.Entities;
using TMS.Core.Abstractions;
using TMS.Core.Enums;

namespace TMS.DataAccess.Repositories
{
    public class TsksRepository : ITsksRepository
    {
        private readonly TMSDbContext _context;
        private readonly IUsersRepository _usersRepository;

        public TsksRepository(TMSDbContext context, IUsersRepository usersRepository)
        {
            _context = context;
            _usersRepository = usersRepository;
        }
        public async Task<(Tsk, string)> Get(Guid id)
        {
            var tskEntity = await _context.Tsks
             .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

            if (tskEntity == null) return (null, "Не найден");

            var (tsk, error) = Tsk.Create(
                tskEntity.Id, tskEntity.CreatorId, tskEntity.AssignedUserId,
                tskEntity.Title, tskEntity.Comment,
                tskEntity.Priority, tskEntity.Status,
                tskEntity.StartDate, tskEntity.EndDate);

            return (tsk, error);
        }

        public async Task<List<Tsk>> GetAll(Guid id)
        {
            var userRoles = await _usersRepository.GetUserRoles(id);

            var taskEntities = await _context.Tsks
                .AsNoTracking()
                .ToListAsync();

            if (userRoles.Any())
            {
                List<TskEntity> filteredTasks = new List<TskEntity>();

                foreach (var role in userRoles)
                {
                    switch (role)
                    {
                        case Role.Admin:
                            var adminTasks = await _context.Tsks
                                .AsNoTracking()
                                .ToListAsync();
                            filteredTasks.AddRange(adminTasks);
                            break;
                        case Role.Manager:
                            var managerTasks = await _context.Tsks
                                .AsNoTracking()
                                .Where(t => t.CreatorId == id)
                                .ToListAsync();
                            filteredTasks.AddRange(managerTasks);
                            break;
                        case Role.Performer:
                            var performerTasks = await _context.Tsks
                                .AsNoTracking()
                                .Where(t => t.AssignedUserId == id)
                                .ToListAsync();
                            filteredTasks.AddRange(performerTasks);
                            break;
                    }
                }

                taskEntities = filteredTasks.DistinctBy(t => t.Id).ToList();
            }
            else
            {
                taskEntities = [];
            }

            var tasks = taskEntities
                .Select(t => Tsk.Create(
                    t.Id, t.CreatorId, t.AssignedUserId,
                    t.Title, t.Comment, t.Priority, t.Status,
                    t.StartDate, t.EndDate).Tsk)
                .ToList();

            return tasks;
        }

        public async Task<Guid> Create(Tsk task)
        {
            var tskEntity = new TskEntity
            {
                Id = task.Id,
                CreatorId = task.CreatorId,
                AssignedUserId = task.AssignedUserId,
                Title = task.Title,
                Comment = task.Comment,
                Priority = task.Priority,
                Status = task.Status,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
            };

            await _context.Tsks.AddAsync(tskEntity);

            await RecordHistory(tskEntity, null, tskEntity.CreatorId, "Создано");

            return task.Id;
        }

        public async Task<Guid> Update(Guid id, Tsk tsk)
        {
            var oldTskEntity = await _context.Tsks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            var newTskEntity = new TskEntity
            {
                CreatorId = tsk.CreatorId,
                AssignedUserId = tsk.AssignedUserId,
                Title = tsk.Title,
                Comment = tsk.Comment,
                Priority = tsk.Priority,
                Status = tsk.Status,
                StartDate = tsk.StartDate,
                EndDate = tsk.EndDate,
            };
            await _context.Tsks
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.CreatorId, t => newTskEntity.CreatorId)
                    .SetProperty(t => t.AssignedUserId, t => newTskEntity.AssignedUserId)
                    .SetProperty(t => t.Title, t => newTskEntity.Title)
                    .SetProperty(t => t.Comment, t => newTskEntity.Comment)
                    .SetProperty(t => t.Priority, t => newTskEntity.Priority)
                    .SetProperty(t => t.Status, t => newTskEntity.Status)
                    .SetProperty(t => t.StartDate, t => newTskEntity.StartDate)
                    .SetProperty(t => t.EndDate, t => newTskEntity.EndDate));

            await RecordHistory(oldTskEntity, newTskEntity, tsk.CreatorId, "Обновлено: ");

            return id;
        }

        public async Task<Guid> ChangeStat(Guid id, Tsk tsk)
        {
            var oldTskEntity = await _context.Tsks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            var newTskEntity = new TskEntity
            {
                CreatorId = tsk.CreatorId,
                AssignedUserId = tsk.AssignedUserId,
                Title = tsk.Title,
                Comment = tsk.Comment,
                Priority = tsk.Priority,
                Status = tsk.Status + 1,
                StartDate = tsk.StartDate,
                EndDate = tsk.EndDate,
            };

            await _context.Tsks
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.CreatorId, t => newTskEntity.CreatorId)
                    .SetProperty(t => t.AssignedUserId, t => newTskEntity.AssignedUserId)
                    .SetProperty(t => t.Title, t => newTskEntity.Title)
                    .SetProperty(t => t.Comment, t => newTskEntity.Comment)
                    .SetProperty(t => t.Priority, t => newTskEntity.Priority)
                    .SetProperty(t => t.Status, t => newTskEntity.Status)
                    .SetProperty(t => t.StartDate, t => newTskEntity.StartDate)
                    .SetProperty(t => t.EndDate, t => newTskEntity.EndDate));
            await RecordHistory(oldTskEntity, newTskEntity, tsk.CreatorId, "Обновлено: ");
            return id;

        }
        public async Task<Guid> Delete(Guid id)
        {
            var tskEntity = await _context.Tsks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            await _context.Tsks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();

            await RecordHistory(tskEntity, null, tskEntity.CreatorId, "Удалено");
            return id;
        }


        private async Task RecordHistory(TskEntity oldTskEntity, TskEntity newTskEntity, Guid userId, string actionType)
        {
            var changes = new List<string>();
            Guid tskId = oldTskEntity.Id;

            if (oldTskEntity != null && newTskEntity != null)
            {
                if (oldTskEntity.CreatorId != newTskEntity.CreatorId)
                    changes.Add($"Назначил: '{oldTskEntity.CreatorId}' -> '{newTskEntity.CreatorId}'");
                if (oldTskEntity.AssignedUserId != newTskEntity.AssignedUserId)
                    changes.Add($"Назначено для: '{oldTskEntity.AssignedUserId}' -> '{newTskEntity.AssignedUserId}'");
                if (oldTskEntity.Title != newTskEntity.Title)
                    changes.Add($"Название: '{oldTskEntity.Title}' -> '{newTskEntity.Title}'");
                if (oldTskEntity.Comment != newTskEntity.Comment)
                    changes.Add($"Комментарий: '{oldTskEntity.Comment}' -> '{newTskEntity.Comment}'");
                if (oldTskEntity.Priority != newTskEntity.Priority)
                    changes.Add($"Приоритет: {oldTskEntity.Priority} -> {newTskEntity.Priority}");
                if (oldTskEntity.Status != newTskEntity.Status)
                    changes.Add($"Статус: {oldTskEntity.Status} -> {newTskEntity.Status}");
                if (oldTskEntity.StartDate != newTskEntity.StartDate)
                    changes.Add($"Дата начала: {oldTskEntity.StartDate} -> {newTskEntity.StartDate}");
                if (oldTskEntity.EndDate != newTskEntity.EndDate)
                    changes.Add($"Дата конца: {oldTskEntity.EndDate} -> {newTskEntity.EndDate}");
            }

            var historyEntity = new TskHistoryEntity
            {
                Id = Guid.NewGuid(),
                TskId = tskId,
                UserId = userId,
                ChangeDate = DateTime.UtcNow,
                Changes = $"{actionType} {string.Join(", ", changes)}"
            };


            await _context.TskHistories.AddAsync(historyEntity);
            await _context.SaveChangesAsync();

        }
        public async Task<List<TskHistory>> GetHistory(Guid taskId)
        {
            var historyEntries = await _context.TskHistories
                .Where(h => h.TskId == taskId)
                .OrderByDescending(h => h.ChangeDate)
                .ToListAsync();

            var history = historyEntries.Select(h => TskHistory.Create(
                h.Id,
                h.TskId,
                h.UserId,
                h.ChangeDate,
                h.Changes
            )).ToList();

            return history;
        }
    }
}

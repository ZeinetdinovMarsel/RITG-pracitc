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

            var task = new TaskModel()
            {
                Id = tskEntity.Id,
                CreatorId = tskEntity.CreatorId,
                AssignedUserId = tskEntity.AssignedUserId,
                Title = tskEntity.Title,
                Comment = tskEntity.Comment,
                Priority = tskEntity.Priority,
                Status = tskEntity.Status,
                StartDate = tskEntity.StartDate,
                EndDate = tskEntity.EndDate,
                AcceptDate = DateTime.UtcNow.Date,
                FinishDate = DateTime.UtcNow.Date
            };

            var (tsk, error) = Tsk.Create(task);


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


            var tasksModels = taskEntities
               .Select(t => new TaskModel()
               {
                   Id = t.Id,
                   CreatorId = t.CreatorId,
                   AssignedUserId = t.AssignedUserId,
                   Title = t.Title,
                   Comment = t.Comment,
                   Priority = t.Priority,
                   Status = t.Status,
                   StartDate = t.StartDate,
                   EndDate = t.EndDate,
                   AcceptDate = DateTime.UtcNow.Date,
                   FinishDate = DateTime.UtcNow.Date
               }).ToList();


            var tsks = tasksModels
                .Select(t => Tsk.Create(t).Tsk)
                .ToList();

            return tsks;
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

            var status = Math.Clamp(tsk.Status + 1, 1, 3);

            DateTime acceptDate = oldTskEntity.AcceptDate;
            DateTime finishDate = oldTskEntity.FinishDate;
            if (status == 2)
            {
                acceptDate = DateTime.UtcNow;
            }
            else if (status == 3)
            {
                finishDate = DateTime.UtcNow;
            }
            var newTskEntity = new TskEntity
            {
                CreatorId = tsk.CreatorId,
                AssignedUserId = tsk.AssignedUserId,
                Title = tsk.Title,
                Comment = tsk.Comment,
                Priority = tsk.Priority,
                Status = status,
                StartDate = tsk.StartDate,
                EndDate = tsk.EndDate,
                AcceptDate = acceptDate,
                FinishDate = finishDate
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
                    .SetProperty(t => t.EndDate, t => newTskEntity.EndDate)
                    .SetProperty(t => t.AcceptDate, t => newTskEntity.AcceptDate)
                    .SetProperty(t => t.FinishDate, t => newTskEntity.FinishDate));
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
            _context.SaveChanges();

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

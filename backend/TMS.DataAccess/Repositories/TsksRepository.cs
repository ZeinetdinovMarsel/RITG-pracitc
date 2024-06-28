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
        public async Task<(Tsk,string)> Get(Guid id)
        {
            var tskEntity = await _context.Tsks
             .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

            if (tskEntity == null) return (null, "Не найден");

            var(tsk, error) = Tsk.Create(
                tskEntity.Id, tskEntity.CreatorId, tskEntity.AssignedUserId,
                tskEntity.Title, tskEntity.Comment, 
                tskEntity.Priority, tskEntity.Status,
                tskEntity.StartDate, tskEntity.EndDate);

            return (tsk,error);
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
            await _context.SaveChangesAsync();

            return task.Id;
        }

        public async Task<Guid> Update(Guid id, Tsk tsk)
        {
            await _context.Tsks
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.CreatorId, t => tsk.CreatorId)
                    .SetProperty(t => t.AssignedUserId, t => tsk.AssignedUserId)
                    .SetProperty(t => t.Title, t => tsk.Title)
                    .SetProperty(t => t.Comment, t => tsk.Comment)
                    .SetProperty(t => t.Priority, t => tsk.Priority)
                    .SetProperty(t => t.Status, t => tsk.Status)
                    .SetProperty(t => t.StartDate, t => tsk.StartDate)
                    .SetProperty(t => t.EndDate, t => tsk.EndDate));

            return id;
        }

        public async Task<Guid> ChangeStat(Guid id, Tsk tsk)
        {

            int status = tsk.Status;
            status = Math.Clamp(++status, 1, 3);
            await _context.Tsks
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.CreatorId, t => tsk.CreatorId)
                    .SetProperty(t => t.AssignedUserId, t => tsk.AssignedUserId)
                    .SetProperty(t => t.Title, t => tsk.Title)
                    .SetProperty(t => t.Comment, t => tsk.Comment)
                    .SetProperty(t => t.Priority, t => tsk.Priority)
                    .SetProperty(t => t.Status, t => status)
                    .SetProperty(t => t.StartDate, t => tsk.StartDate)
                    .SetProperty(t => t.EndDate, t => tsk.EndDate));

            return id;
        }
        public async Task<Guid> Delete(Guid id)
        {
            await _context.Tsks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}

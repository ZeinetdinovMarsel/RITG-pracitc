using Microsoft.EntityFrameworkCore;
using TMS.Core.Models;
using TMS.DataAccess.Entities;
using TMS.Core.Abstractions;

namespace TMS.DataAccess.Repositories
{
    public class TsksRepository : ITsksRepository
    {
        private readonly TMSDbContext _context;
        public TsksRepository(TMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tsk>> Get()
        {
            var taskEntitites = await _context.Tsks
                .AsNoTracking()
                .ToListAsync();

            var Tsks = taskEntitites
                .Select(t => Tsk.Create(t.Id, t.Title, t.Description,
                t.AssignedUserId, t.Priority, t.Status,
                t.StartDate, t.EndDate).Tsk)
                .ToList();

            return Tsks;
        }
        public async Task<Guid> Create(Tsk task)
        {
            var tskEntity = new TskEntity
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AssignedUserId = task.AssignedUserId,
                Priority = task.Priority,
                Status = task.Status,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
            };

            await _context.Tsks.AddAsync(tskEntity);
            await _context.SaveChangesAsync();

            return task.Id;
        }

        public async Task<Guid> Update(Guid id, string title,
            string description, int assignedUserId,
            string priority, string status,
            DateTime startDate, DateTime endDate)
        {
            await _context.Tsks
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.Title, t => title)
                    .SetProperty(t => t.Description, t => description)
                    .SetProperty(t => t.AssignedUserId, t => assignedUserId)
                    .SetProperty(t => t.Priority, t => priority)
                    .SetProperty(t => t.Status, t => status)
                    .SetProperty(t => t.StartDate, t => startDate)
                    .SetProperty(t => t.EndDate, t => endDate));

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

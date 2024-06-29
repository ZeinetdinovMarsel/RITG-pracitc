using Microsoft.EntityFrameworkCore;
using TMS.Core.Abstractions;
using TMS.Core.Enums;
using TMS.Core.Models;
using TMS.DataAccess.Entities;

namespace TMS.DataAccess.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly TMSDbContext _context;

        public AdminRepository(TMSDbContext context)
        {
            _context = context;

        }

        public async Task<Guid> Update(Guid id, User user, Role role)
        {

            var newUserEntity = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == id)
            ?? throw new InvalidOperationException("Пользователь не найден");


            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Id, u => newUserEntity.Id)
                    .SetProperty(u => u.UserName, u => user.UserName)
                    .SetProperty(u => u.Email, u => user.Email));

            await _context.UserRoleEntity
                .Where(u => u.UserId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.RoleId, u => (int)role));


            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var userEntity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}

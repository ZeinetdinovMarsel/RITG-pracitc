using TMS.DataAccess;
using TMS.Core.Models;
using TMS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using TMS.Core.Abstractions;
using TMS.Core.Enums;
namespace TMS.DataAccess.Repositories;
public class UsersRepository : IUsersRepository
{
    private readonly TMSDbContext _context;

    public UsersRepository(TMSDbContext context)
    {
        _context = context;

    }
    public async Task Add(User user)
    {
        var roleEntity = await _context.Roles
            .SingleOrDefaultAsync(r => r.Id == (int)Role.Manager)
            ?? throw new InvalidOperationException();

        var userEntity = new UserEntity()
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception();

        User user = User.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, email);

        return user;
    }

    public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
    {
        var roles = await _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
    }
}
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
    public async Task Add(User user, int role)
    {
        var roleEntity = await _context.Roles
            .SingleOrDefaultAsync(r => r.Id == role)
            ?? throw new InvalidOperationException("Role not found");

        var userEntity = new UserEntity()
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            Roles = [roleEntity]
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == email);

        if (userEntity == null) return null;

        User user = User.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, email);

        return user;
    }
    public async Task<User> GetById(string Id)
    {

        if (!Guid.TryParse(Id, out Guid id))
        {
            throw new ArgumentException("Invalid Id format");
        }
        var userEntity = await _context.Users
       .AsNoTracking()
       .FirstOrDefaultAsync(u => u.Id == id);

        if (userEntity == null) return null;

        User user = User.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, userEntity.Email);

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
    public async Task<List<User>> Get()
    {
        var userEntitites = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        var Users = userEntitites
            .Select(u => User.Create(u.Id, u.UserName, u.PasswordHash, u.Email))
            .ToList();

        return Users;
    }
}
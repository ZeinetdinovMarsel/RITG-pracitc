using TMS.DataAccess;
using TMS.Core.Models;
using TMS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using TMS.Core.Abstractions;
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
}
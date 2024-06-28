using TMS.Core.Enums;
using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task Add(User user, int role);
        Task<User> GetByEmail(string email);
        Task<User> GetById(Guid Id);
        Task<List<User>> GetUsersByRole(int role);
        Task<HashSet<Permission>> GetUserPermissions(Guid userId);
        Task<List<Role>> GetUserRoles(Guid userId);
    }
}
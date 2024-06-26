using TMS.Core.Enums;
using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task Add(User user);
        Task<User> GetByEmail(string email);
        Task<HashSet<Permission>> GetUserPermissions(Guid userId);
    }
}
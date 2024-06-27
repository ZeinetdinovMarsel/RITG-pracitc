using TMS.Core.Enums;
using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task Add(User user, int role);
        Task<User> GetByEmail(string email);
        Task<User> GetById(string Id);

        Task<List<User>> Get();
        Task<HashSet<Permission>> GetUserPermissions(Guid userId);
    }
}
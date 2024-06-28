using TMS.Core.Enums;
using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface IUsersService
    {
        Task<List<User>> GetAllUsersByRole(int role);
        Task<User> GetUserFromToken(string token);
        Task<Role> GetUserRole(Guid id);
        Task<string> Login(string email, string password);
        Task Register(string userName, string email, string password, int role);
    }
}
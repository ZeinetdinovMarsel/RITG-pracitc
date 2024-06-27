
using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface IUsersService
    {
        Task<string> Login(string email, string password);
        Task Register(string userName, string email, string password, int role);

        Task<List<User>> GetAllUsers();
        Task<User> GetUserFromToken(string token);
    }
}
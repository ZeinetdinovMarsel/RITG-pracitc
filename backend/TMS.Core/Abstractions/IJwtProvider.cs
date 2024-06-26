using TMS.Core.Models;

namespace TMS.Core.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
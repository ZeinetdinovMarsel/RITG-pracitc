using TMS.Core.Abstractions;
using TMS.Core.Enums;
using TMS.DataAccess.Repositories;

namespace TMS.Application;

public class PermissionService :  IPermissionService
{
    private readonly IUsersRepository _usersRepository;

    public PermissionService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    public Task<HashSet<Permission>> GetPermissionsAsync(Guid userId)
    {

        return _usersRepository.GetUserPermissions(userId);
    }
}
using TMS.Core.Abstractions;
using TMS.Core.Enums;
using TMS.Core.Models;
using TMS.DataAccess.Repositories;

namespace TMS.Application.Services
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Guid> UpdateUser(Guid id, User user, Role role)
        {
            return await _adminRepository.Update(id, user, role);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _adminRepository.Delete(id);
        }
    }
}

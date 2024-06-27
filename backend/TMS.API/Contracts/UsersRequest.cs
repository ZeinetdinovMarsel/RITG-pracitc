using System.ComponentModel.DataAnnotations;

namespace TMS.API.Contracts
{
    public record UsersRequest
    (
        [Required] Guid UserId,
        [Required] string name
    );

}

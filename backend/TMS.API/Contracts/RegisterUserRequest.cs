using System.ComponentModel.DataAnnotations;

namespace TMS.API.Contracts
{
    public record RegisterUserRequest
    (
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email
    );
}

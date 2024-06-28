using System.ComponentModel.DataAnnotations;

namespace TMS.API.Contracts
{
    public record RoleRequest
    (
        [Required] int role
        );


}

using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace TMS.API.Contracts
{
    public record LoginUserRequest
    (
        [Required] string Email,
        [Required] string Password);


}

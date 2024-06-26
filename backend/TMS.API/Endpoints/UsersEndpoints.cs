using Microsoft.AspNetCore.Mvc;
using TMS.API.Contracts;
using TMS.Application.Services;

namespace TMS.API.Endpoints
{
    public static class UsersEndpoints
    {

        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("register", Register);

            app.MapPost("login", Login);

            return app;
        }
        private static async Task<IResult> Register(
            RegisterUserRequest request,
            UsersService usersService)
        {

            await usersService.Register(
                request.UserName,
                request.Email,
                request.Password);
            return Results.Ok();

        }

        private static async Task<IResult> Login(
            LoginUserRequest request,
            UsersService usersService,
            HttpContext context)
        {
            var token = await usersService.Login(request.Email, request.Password);

            context.Response.Cookies.Append("jwt",token);
            return Results.Ok(token);

        }
    }
}
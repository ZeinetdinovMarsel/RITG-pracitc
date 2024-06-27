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

            app.MapPost("logout", Logout);

            app.MapGet("user", GetUserDetails);

            app.MapGet("users", GetAllUsers);
            return app;
        }
        private static async Task<IResult> Register(
            RegisterUserRequest request,
            UsersService usersService)
        {

            try
            {
                await usersService.Register(
                    request.UserName,
                    request.Email,
                    request.Password,
                    request.Role);
                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { Message = ex.Message });
            }

        }

        private static async Task<IResult> Login(
            LoginUserRequest request,
            UsersService usersService,
            HttpContext context)
        {
            try
            {
                var token = await usersService.Login(request.Email, request.Password);
                context.Response.Cookies.Append("jwt", token);
                return Results.Ok(token);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { Message = ex.Message });
            }

        }

        private static IResult Logout(HttpContext context)
        {
            try
            {
                context.Response.Cookies.Delete("jwt");
                return Results.Ok(new { Message = "Вы успешно вышли" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { Message = ex.Message });
            }
        }

        private static async Task<IResult> GetUserDetails(
            UsersService usersService,
            HttpContext context)
        {
            try
            {
                var token = context.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(token))
                    return Results.BadRequest(new { Message = "Вы не авторизованы" });

                var user = await usersService.GetUserFromToken(token);
                if (user == null)
                    return Results.NotFound(new { Message = "Пользователь не найден" });

                return Results.Ok(user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { Message = ex.Message });
            }
        }

        private static async Task<IResult> GetAllUsers(
          UsersService usersService)
        {
            var users = await usersService.GetAllUsers();

            var response = users.Select(u => new UsersRequest(u.Id,u.UserName));

            return Results.Ok(response);
        }
    }
}
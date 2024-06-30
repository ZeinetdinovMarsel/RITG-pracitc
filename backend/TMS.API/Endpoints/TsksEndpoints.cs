using Microsoft.AspNetCore.Mvc;
using TMS.API.Contracts;
using TMS.Core.Abstractions;
using TMS.Core.Models;
using TMS.API.Extentions;
using TMS.Core.Enums;
using TMS.Application.Services;

namespace TMS.API.Endpoints
{
    public static class TsksEndpoints
    {
        public static IEndpointRouteBuilder MapTsksEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("tsks", GetTsksByUser).RequirePermissions(Permission.Read);
            app.MapPost("tsks", CreateTsk).RequirePermissions(Permission.Create);
            app.MapPut("tsks/{id:guid}", UpdateTsk).RequirePermissions(Permission.Update);
            app.MapDelete("tsks/{id:guid}", DeleteTsk).RequirePermissions(Permission.Delete);
            app.MapPut("tsks/status/change/{id:guid}", StatusChangeTsk).RequirePermissions(Permission.Change);
            app.MapGet("tsks/history", GetTskHisory).RequirePermissions(Permission.Read);
            return app;
        }

        private static async Task<IResult> GetTsksByUser(
            ITsksService tsksService,
            UsersService usersService,
            HttpContext context)
        {
            var token = context.Request.Cookies["jwt"];

            var user = await usersService.GetUserFromToken(token);

            var tsks = await tsksService.GetAllTsksById(user.Id);

            var response = tsks.Select(t => new TsksResponse(
                t.Id,
                t.CreatorId,
                t.AssignedUserId,
                t.Title,
                t.Comment,
                t.Priority,
                t.Status,
                t.StartDate,
                t.EndDate));

            return Results.Ok(response);
        }

        private static async Task<IResult> CreateTsk(
            [FromBody] TsksRequest request,
            ITsksService tsksService,
            UsersService usersService,
            HttpContext context)
        {

            var token = context.Request.Cookies["jwt"];

            var user = await usersService.GetUserFromToken(token);

            var (tsk, error) = Tsk.Create(
                Guid.NewGuid(),
                user.Id,
                request.AssignedUserId,
                request.Title,
                request.Comment,
                request.Priority,
                request.Status,
                request.StartDate,
                request.EndDate,
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date);

            if (!string.IsNullOrEmpty(error))
                return Results.BadRequest(error);

            var bookId = tsksService.CreateTsk(tsk);

            return Results.Ok(tsk);
        }

        private static async Task<IResult> UpdateTsk(
            [FromBody] TsksRequest request,
            ITsksService tsksService,
            Guid id,
            UsersService usersService,
            HttpContext context)
        {

            var token = context.Request.Cookies["jwt"];

            var user = await usersService.GetUserFromToken(token);

            var (tsk, error) = Tsk.Create(
                id,
                user.Id,
                request.AssignedUserId,
                request.Title,
                request.Comment,
                request.Priority,
                request.Status,
                request.StartDate,
                request.EndDate,
               DateTime.UtcNow.Date,
                DateTime.UtcNow.Date);


            if (!string.IsNullOrEmpty(error))
                return Results.BadRequest(error);

            var tskId = await tsksService.UpdateTsk(id, tsk);
            return Results.Ok(tskId);
        }

        private static async Task<IResult> DeleteTsk(
            ITsksService tsksService,
            Guid id)
        {
            return Results.Ok(await tsksService.DeleteTsk(id));
        }

        private static async Task<IResult> StatusChangeTsk(
            ITsksService tsksService,
            Guid id,
            UsersService usersService,
            HttpContext context)
        {

            var token = context.Request.Cookies["jwt"];

            var user = await usersService.GetUserFromToken(token);

            var (tsk, error) = await tsksService.GetTskById(id);

            if (!string.IsNullOrEmpty(error))
                return Results.BadRequest(error);

            var tskId = await tsksService.ChangeTskStat(id, tsk);
            return Results.Ok(tskId);
        }
        private static async Task<IResult> GetTskHisory(
            ITsksService tsksService,
            Guid id
            )
        {
            var history = await tsksService.GetTskHistoryById(id);

            var response = history.Select(h => new TsksHistoryResponse(
                h.Id,
                h.TskId,
                h.UserId,
                h.ChangeDate,
                h.Changes));

            return Results.Ok(response);
        }
    }
}

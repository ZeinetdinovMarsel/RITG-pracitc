using Microsoft.AspNetCore.Mvc;
using TMS.API.Contracts;
using TMS.Core.Abstractions;
using TMS.Core.Models;
using TMS.API.Extentions;
using TMS.Core.Enums;
using TMS.Application.Services;
using TMS.DataAccess.Entities;

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

            var task = new TaskModel()
            {
                Id = Guid.NewGuid(),
                CreatorId = user.Id,
                AssignedUserId = request.AssignedUserId,
                Title = request.Title,
                Comment = request.Comment,
                Priority = request.Priority,
                Status = request.Status,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                AcceptDate = DateTime.UtcNow.Date,
                FinishDate = DateTime.UtcNow.Date
            };

            var (tsk, error) = Tsk.Create(task);


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
            
            var task = new TaskModel()
            {
                Id = id,
                CreatorId = request.CreatorId,
                AssignedUserId = request.AssignedUserId,
                Title= request.Title,
                Comment = request.Comment,
                Priority = request.Priority,
                Status = request.Status,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                AcceptDate = DateTime.UtcNow.Date,
                FinishDate = DateTime.UtcNow.Date
            };

            var (tsk, error) = Tsk.Create(task);


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

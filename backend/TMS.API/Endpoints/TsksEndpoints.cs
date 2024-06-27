using Microsoft.AspNetCore.Mvc;
using TMS.API.Contracts;
using TMS.Core.Abstractions;
using TMS.Core.Models;
using TMS.API.Extentions;
using TMS.Core.Enums;

namespace TMS.API.Endpoints
{
    public static class TsksEndpoints
    {
        public static IEndpointRouteBuilder MapTsksEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("tsks", GetTsks);
            app.MapPost("tsks", CreateTsk).RequirePermissions(Permission.Create);
            app.MapPut("tsks/{id:guid}", UpdateTsk).RequirePermissions(Permission.Update);
            app.MapDelete("tsks/{id:guid}", DeleteTsk).RequirePermissions(Permission.Delete);

            return app;
        }

        private static async Task<IResult> GetTsks(
            ITsksService tsksService)
        {
            var tsks = await tsksService.GetAllTsks();

            var response = tsks.Select(t => new TsksResponse(t.Id, t.Title, t.Comment, t.AssignedUserId, t.Priority, t.Status, t.StartDate, t.EndDate));

            return Results.Ok(response);
        }

        private static async Task<IResult> CreateTsk(
            [FromBody] TsksRequest request,
            ITsksService tsksService)
        {

            var (tsk, error) = Tsk.Create(
                Guid.NewGuid(),
                request.Title,
                request.Comment,
                request.AssignedUserId,
                request.Priority,
                request.Status,
                request.StartDate,
                request.EndDate);

            if (!string.IsNullOrEmpty(error))
                return Results.BadRequest(error);

            var bookId = tsksService.CreateTsk(tsk);

            return Results.Ok(tsk);
        }

        private static async Task<IResult> UpdateTsk(
            [FromBody] TsksRequest request,
            ITsksService tsksService, 
            Guid id)
        {
            var tskId = await tsksService.UpdateTsk(id, request.Title, request.Comment, request.AssignedUserId, request.Priority,
                request.Status, request.StartDate, request.EndDate);
            return Results.Ok(tskId);
        }

        private static async Task<IResult> DeleteTsk(
            ITsksService tsksService,
            Guid id)
        {
            return Results.Ok(await tsksService.DeleteTsk(id));
        }
    }
}

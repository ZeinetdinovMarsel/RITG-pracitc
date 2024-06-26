using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using TMS.API.Contracts;
using TMS.Application.Services;
using TMS.Core.Abstractions;
using TMS.Core.Models;
using Azure.Core;

namespace TMS.API.Endpoints
{
    public static class TsksEndpoints
    {
        public static IEndpointRouteBuilder MapTsksEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("tsks", GetTsks);
            app.MapPost("tsks", CreateTsk).RequireAuthorization();
            app.MapPut("tsks/{id:guid}", UpdateTsk).RequireAuthorization();
            app.MapDelete("tsks/{id:guid}", DeleteTsk);

            return app;
        }

        private static async Task<IResult> GetTsks(
            ITsksService tsksService)
        {
            var tsks = await tsksService.GetAllTsks();

            var response = tsks.Select(t => new TsksResponse(t.Id, t.Title, t.Description, t.AssignedUserId, t.Priority, t.Status, t.StartDate, t.EndDate));

            return Results.Ok(response);
        }

        private static async Task<IResult> CreateTsk(
            [FromBody] TsksRequest request,
            ITsksService tsksService,
            HttpContext context)
        {

            var (tsk, error) = Tsk.Create(
                Guid.NewGuid(),
                request.Title,
                request.Description,
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
            Guid id,
            HttpContext context)
        {
            var tskId = await tsksService.UpdateTsk(id, request.Title, request.Description, request.AssignedUserId, request.Priority,
                request.Status, request.StartDate, request.EndDate);
            return Results.Ok(tskId);
        }

        private static async Task<IResult> DeleteTsk(
            ITsksService tsksService,
            Guid id,
            HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(await tsksService.DeleteTsk(id));
        }
    }
}

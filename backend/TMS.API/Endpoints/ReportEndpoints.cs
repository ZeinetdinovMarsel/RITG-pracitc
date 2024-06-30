using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TMS.API.Contracts;
using TMS.API.Extentions;
using TMS.Application.Services;
using TMS.Core.Abstractions;
using TMS.Core.Enums;
using TMS.Core.Models;
using TMS.DataAccess.Repositories;

namespace TMS.API.Endpoints
{
    public static class ReportEndpoints
    {

        public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/report", GenerateReport).RequirePermissions(Permission.Read);
            return app;
        }
        private static async Task<IResult> GenerateReport(
             ITsksService tsksService,
            UsersService usersService,
            HttpContext context)
        {
            var token = context.Request.Cookies["jwt"];

            var user = await usersService.GetUserFromToken(token);

            var tsks = await tsksService.GetAllTsksById(user.Id);

            double averageCompletionTime = 0;
            var completedTasks = tsks.Where(t => t.Status == (int)Status.Finished).ToList();


            if (completedTasks.Count > 0)
            {
                averageCompletionTime = Math.Round(completedTasks.Average(t => (t.FinishDate - t.AcceptDate).TotalSeconds), 2);
            }

            DateTime today =  DateTime.UtcNow.Date;

            var report = new ReportResponse
            (
                today,
                tsks.Count,
                tsks.Count(t => t.Status == (int)Status.NotAccepted),
                tsks.Count(t => t.Status == (int)Status.Started),
                tsks.Count(t => t.Status == (int)Status.Finished),
                tsks.Count(t => t.Priority == 3),
                tsks.Count(t => t.Priority == 2),
                tsks.Count(t => t.Priority == 1),
                tsks.Count(t => t.EndDate.Date < today.Date),
                averageCompletionTime

            );

            return Results.Ok(report);
        }




    }
}
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TMS.API.Contracts;
using TMS.Application.Services;
using TMS.Core.Models;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TsksController : ControllerBase
    {
        private readonly ITsksService _tsksService;
        public TsksController(ITsksService tsksService)
        {
            _tsksService = tsksService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TsksResponse>>> GetTsks()
        {

            var tsks = await _tsksService.GetAllTsks();

            var response = tsks.Select(t => new TsksResponse(t.Id, t.Title, t.Description, t.AssignedUserId, t.Priority, t.Status, t.StartDate, t.EndDate));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateBook([FromBody] TsksRequest request)
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

                return BadRequest(error);

            var bookId = await _tsksService.CreateTsk(tsk);

            return Ok(tsk);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateTsks(Guid id, [FromBody] TsksRequest request)
        {
            var tskId = await _tsksService.UpdateTsk(id, request.Title, request.Description, request.AssignedUserId, request.Priority,
                request.Status, request.StartDate, request.EndDate);
            return Ok(tskId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteTsk(Guid id)
        {
            return Ok(await _tsksService.DeleteTsk(id));
        }
    }
}
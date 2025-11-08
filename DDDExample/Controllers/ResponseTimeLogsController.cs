using Microsoft.AspNetCore.Mvc;
using DDDExample.Domain.Repositories;

namespace DDDExample.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseTimeLogsController : ControllerBase
    {
        private readonly IResponseTimeLogRepository _repository;

        public ResponseTimeLogsController(IResponseTimeLogRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtener logs de tiempo de respuesta filtrados (opcional)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] string? path = null,
            [FromQuery] string? method = null,
            [FromQuery] int? minDurationMs = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int limit = 100)
        {
            var logs = await _repository.GetLogsAsync(
                path,
                method,
                minDurationMs,
                startDate,
                endDate,
                limit
            );

            return Ok(logs);
        }
    }
}

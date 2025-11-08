using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDExample.Domain.Models;
using DDDExample.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DDDExample.API.Controllers
{
    [ApiController]
    [Route("api/metrics/memory")]
    public class MetricsController : ControllerBase
    {
        private readonly IMemoryMetricsRepository _metricsRepository;

        public MetricsController(IMemoryMetricsRepository metricsRepository)
        {
            _metricsRepository = metricsRepository;
        }

        // GET: /api/metrics/memory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemoryMetric>>> GetRecentMetrics()
        {
            var metrics = await _metricsRepository.GetRecentMetricsAsync();
            return Ok(metrics);
        }

        // GET: /api/metrics/memory/stats
        [HttpGet("stats")]
        public async Task<ActionResult<MemoryMetricsStats>> GetMemoryStatistics()
        {
            // Obtenemos todas las métricas recientes
            var metrics = await _metricsRepository.GetRecentMetricsAsync(1000); // o la cantidad que desees

            if (!metrics.Any())
            {
                return Ok(new MemoryMetricsStats());
            }

            var stats = new MemoryMetricsStats
            {
                AverageMemoryMB = metrics.Average(m => m.ProcessMemoryMB),
                MaxMemoryMB = metrics.Max(m => m.ProcessMemoryMB),
                MinMemoryMB = metrics.Min(m => m.ProcessMemoryMB),
                WarningCount = metrics.Count(m => m.Status == "Warning"),
                CriticalCount = metrics.Count(m => m.Status == "Critical"),
                TotalSamples = metrics.Count()
            };

            return Ok(stats);
        }
    }

    // Clase de estadísticas
    public class MemoryMetricsStats
    {
        public double AverageMemoryMB { get; set; }
        public double MaxMemoryMB { get; set; }
        public double MinMemoryMB { get; set; }
        public int WarningCount { get; set; }
        public int CriticalCount { get; set; }
        public int TotalSamples { get; set; }
    }
}

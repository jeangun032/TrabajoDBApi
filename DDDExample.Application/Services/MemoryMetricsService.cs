using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;



namespace DDDExample.Application.Services
{
    public class MemoryMetricsService : BackgroundService
    {
        private readonly ILogger<MemoryMetricsService> _logger;
        private readonly MemoryMetricsSettings _settings;
        private readonly IMemoryMetricsRepository _metricsRepository;
        private readonly PerformanceCounter _cpuCounter;

        public MemoryMetricsService(
            ILogger<MemoryMetricsService> logger,
            IOptions<MemoryMetricsSettings> settings,
            IMemoryMetricsRepository metricsRepository)
        {
            _logger = logger;
            _settings = settings.Value;
            _metricsRepository = metricsRepository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de métricas de memoria iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var process = Process.GetCurrentProcess();
                    var memoryMB = process.WorkingSet64 / 1024.0 / 1024.0;
                    var gcMemory = GC.GetTotalMemory(forceFullCollection: false) / 1024.0 / 1024.0;
                    var cpuUsage = await GetCpuUsage();

                    var status = memoryMB >= _settings.CriticalThresholdMB 
                        ? "Critical" 
                        : memoryMB >= _settings.WarningThresholdMB 
                            ? "Warning" 
                            : "Normal";

                    _logger.LogInformation(
                        "Uso de memoria - Proceso: {ProcessMemory} MB, GC: {GCMemory} MB, CPU: {CpuUsage}%, Status: {Status}",
                        memoryMB,
                        gcMemory,
                        cpuUsage,
                        status);

                    await _metricsRepository.AddAsync(new MemoryMetric
                    {
                        ProcessMemoryMB = memoryMB,
                        GCMemoryMB = gcMemory,
                        CpuUsage = cpuUsage,
                        Status = status
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al recolectar métricas de memoria");
                }

                await Task.Delay(TimeSpan.FromSeconds(_settings.CollectionIntervalSeconds), stoppingToken);
            }
        }

        private async Task<float> GetCpuUsage()
        {
            _cpuCounter.NextValue();
            await Task.Delay(500);
            return _cpuCounter.NextValue();
        }
    }
}

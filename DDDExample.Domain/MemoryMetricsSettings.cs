namespace DDDExample.Application.Services
{
    public class MemoryMetricsSettings
    {
        public int CollectionIntervalSeconds { get; set; } = 30;
        public double WarningThresholdMB { get; set; } = 500;
        public double CriticalThresholdMB { get; set; } = 800;
    }
}

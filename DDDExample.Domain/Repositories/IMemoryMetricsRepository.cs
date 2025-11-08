using System.Collections.Generic;
using System.Threading.Tasks;
using DDDExample.Domain.Models;

namespace DDDExample.Domain.Repositories
{
    public interface IMemoryMetricsRepository
    {
        // Agrega una métrica de memoria
        Task AddAsync(MemoryMetric metric);

        // Obtiene las métricas más recientes (por defecto 100)
        Task<IEnumerable<MemoryMetric>> GetRecentMetricsAsync(int limit = 100);
    }
}

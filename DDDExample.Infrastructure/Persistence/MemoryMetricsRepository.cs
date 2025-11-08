using System.Collections.Generic;
using System.Threading.Tasks;
using DDDExample.Domain.Models;
using DDDExample.Domain.Repositories;
using MongoDB.Driver;

namespace DDDExample.Infrastructure.Persistence.MongoDB.Repositories
{
    public class MemoryMetricsRepository : IMemoryMetricsRepository
    {
        private readonly MongoDBContext _context;

        public MemoryMetricsRepository(MongoDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MemoryMetric metric)
        {
            await _context.MemoryMetrics.InsertOneAsync(metric);
        }

        public async Task<IEnumerable<MemoryMetric>> GetRecentMetricsAsync(int limit = 100)
        {
            var sort = Builders<MemoryMetric>.Sort.Descending(m => m.Timestamp);
            return await _context.MemoryMetrics
                .Find(FilterDefinition<MemoryMetric>.Empty)
                .Sort(sort)
                .Limit(limit)
                .ToListAsync();
        }
    }
}

#nullable enable
using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDExample.Infrastructure.Repositories
{
    public class ResponseTimeLogRepository : IResponseTimeLogRepository
    {
        private readonly IMongoCollection<ResponseTimeLog> _logsCollection;

        public ResponseTimeLogRepository(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _logsCollection = mongoDatabase.GetCollection<ResponseTimeLog>("ResponseTimeLogs");
        }

        public async Task AddAsync(ResponseTimeLog log)
        {
            await _logsCollection.InsertOneAsync(log);
        }

        public async Task<IEnumerable<ResponseTimeLog>> GetLogsAsync(
            string? path = null,
            string? method = null,
            int? minDurationMs = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int limit = 100)
        {
            var filterBuilder = Builders<ResponseTimeLog>.Filter;
            var filter = filterBuilder.Empty;

            if (path != null)
                filter &= filterBuilder.Eq(l => l.Path, path);

            if (method != null)
                filter &= filterBuilder.Eq(l => l.Method, method);

            if (minDurationMs.HasValue)
                filter &= filterBuilder.Gte(l => l.DurationMs, minDurationMs);

            if (startDate.HasValue)
                filter &= filterBuilder.Gte(l => l.Timestamp, startDate);

            if (endDate.HasValue)
                filter &= filterBuilder.Lte(l => l.Timestamp, endDate);

            return await _logsCollection
                .Find(filter)
                .SortByDescending(l => l.Timestamp)
                .Limit(limit)
                .ToListAsync();
        }
    }
}

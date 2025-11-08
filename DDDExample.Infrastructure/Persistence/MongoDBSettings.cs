using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DDDExample.Infrastructure.Persistence.MongoDB
{
    public class MongoDBContext
    {
        public IMongoDatabase Database { get; }

        public MongoDBContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);
        }
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}







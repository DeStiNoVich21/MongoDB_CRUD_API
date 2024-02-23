using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB_CRUD_API.Configuration;
using MongoDB_CRUD_API.Models;

namespace MongoDB_CRUD_API.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbConfiguration> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Employee> CSharpCornerArticles =>
            _database.GetCollection<Employee>("employees");
    }
}

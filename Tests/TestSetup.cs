// using Microsoft.Extensions.Options;
// using MongoDB.Driver;


// namespace MARKETPLACEAPI.Services;

// public class MongoDbContext
// {
//     private readonly IMongoDatabase _database = null;

//     public MongoDbContext(string connectionString, string databaseName)
//     {
//         var client = new MongoClient(connectionString);
//         if (client != null)
//             _database = client.GetDatabase(databaseName);
//     }

//     public IMongoCollection<T> GetCollection<T>(string name)
//     {
//         return _database.GetCollection<T>(name);
//     }
// }
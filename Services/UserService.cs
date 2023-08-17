using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(
        IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
    {
        DatabaseConfig databaseConfig = new();

        string connectionString = databaseConfig.ConnectionString;
        
        var mongoClient = new MongoClient(
            connectionString);


        var mongoDatabase = mongoClient.GetDatabase(
            marketPlaceDBSettings.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            marketPlaceDBSettings.Value.UserCollectionName);
    }

    public async Task<IList<User>> GetAsync() =>
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _usersCollection.Find(x => x.userId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _usersCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _usersCollection.ReplaceOneAsync(x => x.userId == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _usersCollection.DeleteOneAsync(x => x.userId == id);

    public async Task<User?> GetUserByWalletAddress(string walletAddress) =>
        await _usersCollection.Find(x => x.walletAddress == walletAddress).FirstOrDefaultAsync();

    public async Task<bool> UserExists(string walletAddress) =>
        await _usersCollection.Find(x => x.walletAddress == walletAddress).AnyAsync();
}
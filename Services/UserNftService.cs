using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class UserNftService : IUserNftService
{
  private readonly IMongoCollection<UserNft> _userNftCollection;

  public UserNftService(
      IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
  {
    DatabaseConfig databaseConfig = new();

    string connectionString = databaseConfig.ConnectionString;

    var mongoClient = new MongoClient(
        connectionString);

    var mongoDatabase = mongoClient.GetDatabase(
        marketPlaceDBSettings.Value.DatabaseName);

    _userNftCollection = mongoDatabase.GetCollection<UserNft>(
        marketPlaceDBSettings.Value.UserNftCollectionName);
  }

  public async Task<List<UserNft>> GetAsync() =>
      await _userNftCollection.Find(_ => true).ToListAsync();

  public async Task<UserNft?> GetAsync(string id) =>
      await _userNftCollection.Find(x => x.userNftId == id).FirstOrDefaultAsync();

  public async Task CreateAsync(UserNft newUserNft) =>
      await _userNftCollection.InsertOneAsync(newUserNft);

  public async Task UpdateAsync(string id, UserNft updatedUserNft) =>
      await _userNftCollection.ReplaceOneAsync(x => x.userNftId == id, updatedUserNft);

  public async Task RemoveAsync(string id) =>
      await _userNftCollection.DeleteOneAsync(x => x.userNftId == id);

  public async Task<UserNft?> GetUserNftByNftId(string nftId) =>
      await _userNftCollection.Find(x => x.nftId == nftId).FirstOrDefaultAsync();

  public async Task<List<UserNft>> GetUserNftByUserId(string userId) =>
      await _userNftCollection.Find(x => x.userId == userId).ToListAsync();

  public async Task<UserNft?> GetUserNftByUserIdAndNftId(string userId, string nftId) =>
      await _userNftCollection.Find(x => x.userId == userId && x.nftId == nftId).FirstOrDefaultAsync();

}
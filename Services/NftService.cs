using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class NftService : INftService
{
  private readonly IMongoCollection<Nft> _nftCollection;

  public NftService(
      IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
  {
    DatabaseConfig databaseConfig = new();

    string connectionString = databaseConfig.ConnectionString;

    var mongoClient = new MongoClient(
        connectionString);

    var mongoDatabase = mongoClient.GetDatabase(
        marketPlaceDBSettings.Value.DatabaseName);

    _nftCollection = mongoDatabase.GetCollection<Nft>(
        marketPlaceDBSettings.Value.NftCollectionName);
  }

  public async Task<List<Nft>> GetAsync() =>
      await _nftCollection.Find(_ => true).ToListAsync();

  public async Task<Nft?> GetAsync(string id) =>
      await _nftCollection.Find(x => x.nftId == id).FirstOrDefaultAsync();

  public async Task CreateAsync(Nft newNft) =>
      await _nftCollection.InsertOneAsync(newNft);

  public async Task UpdateAsync(string id, Nft updatedNft) =>
      await _nftCollection.ReplaceOneAsync(x => x.nftId == id, updatedNft);

  public async Task RemoveAsync(string id) =>
      await _nftCollection.DeleteOneAsync(x => x.nftId == id);

  public async Task<List<Nft>> GetNftsByCreatorId(string creatorId) =>
      await _nftCollection.Find(x => x.creatorId == creatorId).ToListAsync();
  public async Task<List<Nft>> GetNftsByOwnerId(string ownerId) =>
      await _nftCollection.Find(x => x.ownerId == ownerId).ToListAsync();
  public async Task<Nft?> GetNftByUserIdAndNftId(string userId, string nftId) =>
      await _nftCollection.Find(x => x.ownerId == userId && x.nftId == nftId).FirstOrDefaultAsync();

  // sort price ascending if ascending is true, else descending
  public async Task<List<Nft>> GetNftWithPriceFilter(double? priceMax, double? priceMin, bool? ascending = true)
  {
    var filter = Builders<Nft>.Filter.Empty;
    if (priceMax != null)
    {
      filter &= Builders<Nft>.Filter.Lte(x => x.price, priceMax);
    }
    if (priceMin != null)
    {
      filter &= Builders<Nft>.Filter.Gte(x => x.price, priceMin);
    }
    var sort = Builders<Nft>.Sort.Ascending(x => x.price);
    if (ascending == false)
    {
      sort = Builders<Nft>.Sort.Descending(x => x.price);
    }
    return await _nftCollection.Find(filter).Sort(sort).ToListAsync();
  }

  public async Task<List<Nft>> SearchByNftName(string nftName, bool? ascending = true)
  {
    var filter = Builders<Nft>.Filter.Regex(x => x.nftName, new BsonRegularExpression(nftName, "i"));
    var sort = Builders<Nft>.Sort.Ascending(x => x.price);
    if (ascending == false)
    {
      sort = Builders<Nft>.Sort.Descending(x => x.price);
    }
    return await _nftCollection.Find(filter).Sort(sort).ToListAsync();

  }
}
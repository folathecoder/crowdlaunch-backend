using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class NftLikeService : INftLikeService
{
  private readonly IMongoCollection<NftLike> _nftLikeCollection;
  private readonly IMongoCollection<Nft> _nftCollection;

  public NftLikeService(
      IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
  {
    var mongoClient = new MongoClient(
        marketPlaceDBSettings.Value.ConnectionString);

    var mongoDatabase = mongoClient.GetDatabase(
        marketPlaceDBSettings.Value.DatabaseName);

    _nftLikeCollection = mongoDatabase.GetCollection<NftLike>(
        marketPlaceDBSettings.Value.NftLikeCollectionName);

    _nftCollection = mongoDatabase.GetCollection<Nft>(
        marketPlaceDBSettings.Value.NftCollectionName);
  }

  public async Task<List<NftLike>> GetAsync() =>
      await _nftLikeCollection.Find(_ => true).ToListAsync();

  public async Task<NftLike?> GetAsync(string id) =>
      await _nftLikeCollection.Find(x => x.nftLikeId == id).FirstOrDefaultAsync();

  public async Task CreateAsync(NftLike newNftLike) {
        await _nftLikeCollection.InsertOneAsync(newNftLike);
        var nft = await _nftCollection.Find(x => x.nftId == newNftLike.nftId).FirstOrDefaultAsync();
        nft.noOfLikes += 1;
        await _nftCollection.ReplaceOneAsync(x => x.nftId == nft.nftId, nft);
    }

  public async Task UpdateAsync(string id, NftLike updatedNftLike) =>
        await _nftLikeCollection.ReplaceOneAsync(x => x.nftLikeId == id, updatedNftLike);

  public async Task RemoveAsync(string id) =>
      await _nftLikeCollection.DeleteOneAsync(x => x.nftLikeId == id);

  public async Task<NftLike?> GetNftLikeByNftId(string nftId) =>
      await _nftLikeCollection.Find(x => x.nftId == nftId).FirstOrDefaultAsync();
  

  public async Task<List<NftLike>> GetNftLikesByUserId(string userId) =>
      await _nftLikeCollection.Find(x => x.userId == userId).ToListAsync();
  
  public async Task<NftLike?> GetNftLikeByUserIdAndNftId(string userId, string nftId) =>
      await _nftLikeCollection.Find(x => x.userId == userId && x.nftId == nftId).FirstOrDefaultAsync();
    
    
}
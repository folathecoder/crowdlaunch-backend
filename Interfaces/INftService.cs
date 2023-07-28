using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface INftService : IDefaultService<Nft> {
  Task<List<Nft>> GetNftsByCreatorId(string creatorId);
  Task<List<Nft>> GetNftsByOwnerId(string ownerId);
  Task<Nft?> GetNftByUserIdAndNftId(string userId, string nftId);

  Task<List<Nft>> GetNftWithPriceFilter(double? priceMax, double? priceMin, bool? ascending = true);
  Task<List<Nft>> SearchByNftName(string nftName, bool? ascending = true);
}
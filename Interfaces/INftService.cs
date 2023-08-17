using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface INftService : IDefaultService<Nft> {
  Task<IList<Nft>> GetNftsByCreatorId(string creatorId);
  Task<IList<Nft>> GetNftsByOwnerId(string ownerId);
  Task<Nft?> GetNftByUserIdAndNftId(string userId, string nftId);

  Task<IList<Nft>> GetNftWithPriceFilter(double? priceMax, double? priceMin, bool? ascending = true);
  Task<IList<Nft>> SearchByNftName(string nftName, bool? ascending = true);
}
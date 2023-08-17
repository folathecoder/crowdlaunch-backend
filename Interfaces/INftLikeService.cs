using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface INftLikeService : IDefaultService<NftLike> {
  Task<NftLike?> GetNftLikeByNftId(string nftId);
  Task<IList<NftLike>> GetNftLikesByUserId(string userId);
  Task<NftLike?> GetNftLikeByUserIdAndNftId(string userId, string nftId);
}
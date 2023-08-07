using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IUserNftService : IDefaultService<UserNft> {
  Task<UserNft?> GetUserNftByNftId(string nftId);
  Task<List<UserNft>> GetUserNftByUserId(string userId);
  Task<UserNft?> GetUserNftByUserIdAndNftId(string userId, string nftId);
} 


using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IUserService : IDefaultService<User> {
  Task<User?> GetUserByWalletAddress(string walletAddress);
  Task<bool> UserExists(string walletAddress);
}

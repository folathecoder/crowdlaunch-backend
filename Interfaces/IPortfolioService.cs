using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IPortfolioService : IDefaultService<Portfolio> {
  Task<List<Portfolio>> GetPortfolioByUserId(string userId);

  Task<Portfolio?> GetPortfolioByProjectId(string projectId);

  Task<List<Portfolio>> GetPortfoliosByProjectId(string projectId);

  Task<Portfolio?> GetPortfolioByUserIdAndProjectId(string userId, string projectId);

}
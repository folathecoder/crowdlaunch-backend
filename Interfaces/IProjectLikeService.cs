using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IProjectLikeService : IDefaultService<ProjectLike> {
  Task<List<ProjectLike>> GetProjectLikeByProjectId(string projectId);
  Task<List<ProjectLike>> GetProjectLikesByUserId(string userId);
  Task<ProjectLike?> GetProjectLikeByUserIdAndProjectId(string userId, string projectId);
}
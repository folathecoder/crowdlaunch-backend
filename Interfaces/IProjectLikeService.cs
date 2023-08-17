using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IProjectLikeService : IDefaultService<ProjectLike> {
  Task<IList<ProjectLike>> GetProjectLikeByProjectId(string projectId);
  Task<IList<ProjectLike>> GetProjectLikesByUserId(string userId);
  Task<ProjectLike?> GetProjectLikeByUserIdAndProjectId(string userId, string projectId);
}
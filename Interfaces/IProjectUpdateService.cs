using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IProjectUpdateService : IDefaultService<ProjectUpdate> {
  Task<IList<ProjectUpdate>> GetProjectUpdatesByProjectId(string projectId);
} 


using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IProjectUpdateService : IDefaultService<ProjectUpdate> {
  Task<List<ProjectUpdate>> GetProjectUpdatesByProjectId(string projectId);
} 


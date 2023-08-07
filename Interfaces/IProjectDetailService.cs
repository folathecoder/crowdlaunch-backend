using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IProjectDetailService : IDefaultService<ProjectDetail> {
  Task<ProjectDetail?> GetProjectDetailsByProjectId(string projectId);
} 

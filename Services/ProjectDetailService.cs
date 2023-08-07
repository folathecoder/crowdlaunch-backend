using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class ProjectDetailService : IProjectDetailService
{
  private readonly IMongoCollection<ProjectDetail> _projectDetailCollection;

  public ProjectDetailService(
      IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
  {
    DatabaseConfig databaseConfig = new();

    string connectionString = databaseConfig.ConnectionString;

    var mongoClient = new MongoClient(
        connectionString);

    var mongoDatabase = mongoClient.GetDatabase(
        marketPlaceDBSettings.Value.DatabaseName);

    _projectDetailCollection = mongoDatabase.GetCollection<ProjectDetail>(
        marketPlaceDBSettings.Value.ProjectDetailCollectionName);
  }

  public async Task<List<ProjectDetail>> GetAsync() =>
      await _projectDetailCollection.Find(_ => true).ToListAsync();

  public async Task<ProjectDetail?> GetAsync(string id) =>
      await _projectDetailCollection.Find(x => x.projectDetailId == id).FirstOrDefaultAsync();

  public async Task CreateAsync(ProjectDetail newProjectDetail) =>
      await _projectDetailCollection.InsertOneAsync(newProjectDetail);

  public async Task UpdateAsync(string id, ProjectDetail updatedProjectDetail) =>
      await _projectDetailCollection.ReplaceOneAsync(x => x.projectDetailId == id, updatedProjectDetail);

  public async Task RemoveAsync(string id) =>
      await _projectDetailCollection.DeleteOneAsync(x => x.projectDetailId == id);

  public async Task<ProjectDetail?> GetProjectDetailsByProjectId(string projectId) =>
      await _projectDetailCollection.Find(x => x.projectId == projectId).FirstOrDefaultAsync();

}
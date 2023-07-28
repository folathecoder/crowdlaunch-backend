using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class ProjectUpdateService : IProjectUpdateService
{
    private readonly IMongoCollection<ProjectUpdate> _projectUpdateCollection;

    public ProjectUpdateService(
        IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
    {
        var mongoClient = new MongoClient(
            marketPlaceDBSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            marketPlaceDBSettings.Value.DatabaseName);

        _projectUpdateCollection = mongoDatabase.GetCollection<ProjectUpdate>(
            marketPlaceDBSettings.Value.ProjectUpdateCollectionName);
    }

    public async Task<List<ProjectUpdate>> GetAsync() =>
        await _projectUpdateCollection.Find(_ => true).ToListAsync();

    public async Task<ProjectUpdate?> GetAsync(string id) =>
        await _projectUpdateCollection.Find(x => x.projectUpdateId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ProjectUpdate newProjectUpdate) =>
        await _projectUpdateCollection.InsertOneAsync(newProjectUpdate);

    public async Task UpdateAsync(string id, ProjectUpdate updatedProjectUpdate) =>
        await _projectUpdateCollection.ReplaceOneAsync(x => x.projectUpdateId == id, updatedProjectUpdate);

    public async Task RemoveAsync(string id) =>
        await _projectUpdateCollection.DeleteOneAsync(x => x.projectUpdateId == id);

    public async Task<List<ProjectUpdate>> GetProjectUpdatesByProjectId(string projectId) =>
        await _projectUpdateCollection.Find(x => x.projectId == projectId).ToListAsync();
}
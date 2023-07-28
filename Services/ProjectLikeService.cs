using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class ProjectLikeService : IProjectLikeService
{
    private readonly IMongoCollection<ProjectLike> _projectLikeCollection;

    public ProjectLikeService(
        IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
    {
        var mongoClient = new MongoClient(
            marketPlaceDBSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            marketPlaceDBSettings.Value.DatabaseName);

        _projectLikeCollection = mongoDatabase.GetCollection<ProjectLike>(
            marketPlaceDBSettings.Value.ProjectLikeCollectionName);
    }

    public async Task<List<ProjectLike>> GetAsync() =>
        await _projectLikeCollection.Find(_ => true).ToListAsync();

    public async Task<ProjectLike?> GetAsync(string id) =>
        await _projectLikeCollection.Find(x => x.projectLikeId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ProjectLike newProjectLike) =>
        await _projectLikeCollection.InsertOneAsync(newProjectLike);

    public async Task UpdateAsync(string id, ProjectLike updatedProjectLike) =>
        await _projectLikeCollection.ReplaceOneAsync(x => x.projectLikeId == id, updatedProjectLike);

    public async Task RemoveAsync(string id) =>
        await _projectLikeCollection.DeleteOneAsync(x => x.projectLikeId == id);

    public async Task<List<ProjectLike>> GetProjectLikeByProjectId(string projectId) =>
        await _projectLikeCollection.Find(x => x.projectId == projectId).ToListAsync();
    
    public async Task<List<ProjectLike>> GetProjectLikesByUserId(string userId) => 
        await _projectLikeCollection.Find(x => x.userId == userId).ToListAsync();
    
    public async Task<ProjectLike?> GetProjectLikeByUserIdAndProjectId(string userId, string projectId) =>
        await _projectLikeCollection.Find(x => x.userId == userId && x.projectId == projectId).FirstOrDefaultAsync();
}
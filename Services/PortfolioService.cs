using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IMongoCollection<Portfolio> _portfolioCollection;

    public PortfolioService(
        IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
    {
        var mongoClient = new MongoClient(
            marketPlaceDBSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            marketPlaceDBSettings.Value.DatabaseName);

        _portfolioCollection = mongoDatabase.GetCollection<Portfolio>(
            marketPlaceDBSettings.Value.PortfolioCollectionName);
    }

    public async Task<List<Portfolio>> GetAsync() =>
        await _portfolioCollection.Find(_ => true).ToListAsync();

    public async Task<Portfolio?> GetAsync(string id) =>
        await _portfolioCollection.Find(x => x.portfolioId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Portfolio newPortfolio) =>
        await _portfolioCollection.InsertOneAsync(newPortfolio);

    public async Task UpdateAsync(string id, Portfolio updatedPortfolio) =>
        await _portfolioCollection.ReplaceOneAsync(x => x.portfolioId == id, updatedPortfolio);

    public async Task RemoveAsync(string id) =>
        await _portfolioCollection.DeleteOneAsync(x => x.portfolioId == id);

    public async Task<List<Portfolio>> GetPortfolioByUserId(string userId) =>
        await _portfolioCollection.Find(x => x.userId == userId).ToListAsync();
    

    public async Task<Portfolio?> GetPortfolioByProjectId(string projectId) =>
        await _portfolioCollection.Find(x => x.projectId == projectId).FirstOrDefaultAsync();
    

    public async Task<List<Portfolio>> GetPortfoliosByProjectId(string projectId) =>
        await _portfolioCollection.Find(x => x.projectId == projectId).ToListAsync();

    public async Task<Portfolio?> GetPortfolioByUserIdAndProjectId(string userId, string projectId) => 
        await _portfolioCollection.Find(x => x.userId == userId && x.projectId == projectId).FirstOrDefaultAsync();
    
}
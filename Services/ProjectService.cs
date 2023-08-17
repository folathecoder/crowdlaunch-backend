using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class ProjectService : IProjectService
{
    private readonly IMongoCollection<Project> _projectCollection;
    private readonly IMongoCollection<Portfolio> _portfolioCollection;
    private readonly IMongoCollection<ProjectLike> _projectLikeCollection;
    

    public ProjectService(
        IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
    {
        DatabaseConfig databaseConfig = new();

        string connectionString = databaseConfig.ConnectionString;
        
        var mongoClient = new MongoClient(
            connectionString);


        var mongoDatabase = mongoClient.GetDatabase(
            marketPlaceDBSettings.Value.DatabaseName);

        _projectCollection = mongoDatabase.GetCollection<Project>(
            marketPlaceDBSettings.Value.ProjectCollectionName);
        _portfolioCollection = mongoDatabase.GetCollection<Portfolio>(
            marketPlaceDBSettings.Value.PortfolioCollectionName);
        _projectLikeCollection = mongoDatabase.GetCollection<ProjectLike>(
            marketPlaceDBSettings.Value.ProjectLikeCollectionName);
    }

    public async Task<IList<Project>> GetAsync() =>
        await _projectCollection.Find(_ => true).ToListAsync();

    public async Task<Project?> GetAsync(string id) =>
        await _projectCollection.Find(x => x.projectId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Project newProject) =>
        await _projectCollection.InsertOneAsync(newProject);

    public async Task UpdateAsync(string id, Project updatedProject) =>
        await _projectCollection.ReplaceOneAsync(x => x.projectId == id, updatedProject);

    public async Task RemoveAsync(string id) =>
        await _projectCollection.DeleteOneAsync(x => x.projectId == id);

  public async Task<Project?> GetProjectByWalletAddress(string walletAddress) =>
    await _projectCollection.Find(x => x.projectWalletAddress == walletAddress).FirstOrDefaultAsync();
  

  public async Task<IList<Project>> GetProjectsByUserId(string userId) =>
    await _projectCollection.Find(x => x.userId == userId).ToListAsync();
  

  public async Task<IList<Project>> SearchByProjectName(string projectName, bool? ascending = true) {
    var filter = Builders<Project>.Filter.Regex(x => x.projectName, new BsonRegularExpression(projectName, "i"));
    var sort = Builders<Project>.Sort.Ascending(x => x.createdAt);
    if (ascending == false) {
      sort = Builders<Project>.Sort.Descending(x => x.createdAt);
    }
    return await _projectCollection.Find(filter).Sort(sort).ToListAsync();
  }
  

  public async Task<IList<Project>> GetProjectWithFilters(string? search, bool? newest, bool? trending, Status? active, 
  bool? mostLiked, List<string?> categoryIds, double? minInvestmentMin, double? minInvestmentMax, bool? minInvestmentAsc,
  double? amountRaisedMin, double? amountRaisedMax, bool? amountRaisedAsc, double? targetAmountMin,
  double? targetAmountMax, bool? targetAmountAsc, 
  int? noOfDaysLeftMin, int? noOfDaysLeftMax, bool? noOfDaysLeftAsc, bool? ascending = true)
  {
    var filter = Builders<Project>.Filter.Empty;
    var sort = Builders<Project>.Sort.Descending(x => x.createdAt);
    if (search != null) {
      filter = Builders<Project>.Filter.Regex(x => x.projectName, new BsonRegularExpression(search, "i"));
    }
    if (ascending == false) {
      sort = Builders<Project>.Sort.Ascending(x => x.createdAt);
    }

    if (newest == true) {
      sort = Builders<Project>.Sort.Descending(x => x.createdAt);
    }


    if (trending == true) {

      var projectIdsFromPortfolio = await _portfolioCollection.Find(x => x.createdAt > DateTime.Now.AddDays(-1)).ToListAsync();
      var projectIdsFromProjectLike = await _projectLikeCollection.Find(x => x.createdAt > DateTime.Now.AddDays(-1)).ToListAsync();

      var projectIdsFromPortfolioList = projectIdsFromPortfolio.Select(x => x.projectId).ToList();
      var projectIdsFromProjectLikeList = projectIdsFromProjectLike.Select(x => x.projectId).ToList();

      sort = Builders<Project>.Sort.Descending(x => x.noOfLikes);
      filter &= Builders<Project>.Filter.In(x => x.projectId, projectIdsFromPortfolioList);
      filter &= Builders<Project>.Filter.In(x => x.projectId, projectIdsFromProjectLikeList);
    }

    if (active != null) {
      filter &= Builders<Project>.Filter.Eq(x => x.projectStatus, active);
    }

    if (mostLiked == true) {
      sort = Builders<Project>.Sort.Descending(x => x.noOfLikes);
    }

    if (categoryIds.Count > 0) {
      filter &= Builders<Project>.Filter.In(x => x.categoryId, categoryIds);
    }

    if (minInvestmentMin != null && minInvestmentMax != null) {
      filter &= Builders<Project>.Filter.Gte(x => x.minInvestment, minInvestmentMin);
      filter &= Builders<Project>.Filter.Lte(x => x.minInvestment, minInvestmentMax);
      if (minInvestmentAsc == true) {
        sort = Builders<Project>.Sort.Ascending(x => x.minInvestment);
      }
      else {
        sort = Builders<Project>.Sort.Descending(x => x.minInvestment);
      }
    }

    if (amountRaisedMin != null && amountRaisedMax != null) {
      filter &= Builders<Project>.Filter.Gte(x => x.amountRaised, amountRaisedMin);
      filter &= Builders<Project>.Filter.Lte(x => x.amountRaised, amountRaisedMax);
      if (amountRaisedAsc == true) {
        sort = Builders<Project>.Sort.Ascending(x => x.amountRaised);
      }
      else {
        sort = Builders<Project>.Sort.Descending(x => x.amountRaised);
      }
    }

    if (targetAmountMin != null && targetAmountMax != null) {
      filter &= Builders<Project>.Filter.Gte(x => x.targetAmount, targetAmountMin);
      filter &= Builders<Project>.Filter.Lte(x => x.targetAmount, targetAmountMax);
      if (targetAmountAsc == true) {
        sort = Builders<Project>.Sort.Ascending(x => x.targetAmount);
      }
      else {
        sort = Builders<Project>.Sort.Descending(x => x.targetAmount);
      }
    }

    if (noOfDaysLeftMin != null && noOfDaysLeftMax != null) {
      filter &= Builders<Project>.Filter.Gte(x => x.noOfDaysLeft, noOfDaysLeftMin);
      filter &= Builders<Project>.Filter.Lte(x => x.noOfDaysLeft, noOfDaysLeftMax);
      if (noOfDaysLeftAsc == true) {
        sort = Builders<Project>.Sort.Ascending(x => x.noOfDaysLeft);
      }
      else {
        sort = Builders<Project>.Sort.Descending(x => x.noOfDaysLeft);
      }
    }


    
    return await _projectCollection.Find(filter).Sort(sort).ToListAsync();
  }
}
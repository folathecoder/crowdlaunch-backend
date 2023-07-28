using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MARKETPLACEAPI.Services;

public class CategoryService : IDefaultService<Category>
{
    private readonly IMongoCollection<Category> _categoriesCollection;

    public CategoryService(
        IOptions<MarketPlaceDBSettings> marketPlaceDBSettings)
    {
        var mongoClient = new MongoClient(
            marketPlaceDBSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            marketPlaceDBSettings.Value.DatabaseName);

        _categoriesCollection = mongoDatabase.GetCollection<Category>(
            marketPlaceDBSettings.Value.CategoryCollectionName);
    }

    public async Task<List<Category>> GetAsync() =>
        await _categoriesCollection.Find(_ => true).ToListAsync();

    public async Task<Category?> GetAsync(string id) =>
        await _categoriesCollection.Find(x => x.categoryId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Category newCategory) =>
        await _categoriesCollection.InsertOneAsync(newCategory);

    public async Task UpdateAsync(string id, Category updatedCategory) =>
        await _categoriesCollection.ReplaceOneAsync(x => x.categoryId == id, updatedCategory);

    public async Task RemoveAsync(string id) =>
        await _categoriesCollection.DeleteOneAsync(x => x.categoryId == id);
}
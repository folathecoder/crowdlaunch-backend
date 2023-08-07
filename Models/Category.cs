using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class Category : DefaultModel {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? categoryId { get; set; }
    public string? categoryName { get; set; }
    public string? categoryDescription { get; set; }    
}
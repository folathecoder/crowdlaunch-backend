using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class ProjectLike : DefaultModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? projectLikeId { get; set; }
  public string? userId { get; set; }
  public string? projectId { get; set; }

}
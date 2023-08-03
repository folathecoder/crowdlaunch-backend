using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class ProjectUpdate : DefaultModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? projectUpdateId { get; set; }
  public string? projectId { get; set; }
  public string? updateTitle { get; set; }
  public string? updateMessage { get; set; }

}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class ProjectDetail : DefaultModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? projectDetailId { get; set; }
  public string? projectId { get; set; }
  public string? overview { get; set; }
  public string? competitors { get; set; }
  public string? strategy { get; set; }
  public string? financials { get; set; }
  public string? dividend { get; set; }
  public string? performance { get; set; }
  public string? risks { get; set; }
}
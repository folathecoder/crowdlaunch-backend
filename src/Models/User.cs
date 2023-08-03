using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class User : DefaultModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? userId { get; set; }
  public string? userName { get; set; }
  public string? walletAddress { get; set; }
  public Socials? socials { get; set; }
}
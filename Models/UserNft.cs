using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class UserNft : DefaultModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? userNftId { get; set; }
  public string? userId { get; set; }
  public string? nftId { get; set; }
}
